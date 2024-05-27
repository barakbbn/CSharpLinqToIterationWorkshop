using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace Exercise_5A
{
    public struct Challenge1Results
    {
        public IEnumerable<string> GroupNamesUpperCase;
        public IEnumerable<string> IsraeliGroups;
        public int IsraeliGroupsCount;
        public int TotalEvents;
        public IEnumerable<(string Organizer, string GroupName)> First3Organizers;
    }

    public struct MeetupStats
    {
        public string Name;
        public double AverageAttendees;
        public string LatestEventTitle;
        public DateTime LatestEventDate;
        public bool EverHadAtLeast200Attendees;
        public override string ToString()
        {
            return $"{{Name: {Name}, AverageAttendees: {AverageAttendees}, LatestEventTitle: {LatestEventTitle}, LatestEventDate: {LatestEventDate:g}, EverHadAtLeast200Attendees: {EverHadAtLeast200Attendees}";
        }
    }

    public static class Program
    {
        public static Challenge1Results Challenge1(Meetup.DB meetups)
        {
            return new Challenge1Results
            {
                GroupNamesUpperCase = meetups.Groups.Select(g => g.Name.ToUpper()),
                IsraeliGroups = meetups
                    .Groups.Where(g => g.Name.Contains("Israel"))
                    .Select(g => g.Name),
                IsraeliGroupsCount = meetups.Groups.Count(g => g.Name.Contains("Israel")),
                TotalEvents = meetups.Groups.Sum(g => g.Events.Length),
                First3Organizers = meetups
                    .Groups.OrderBy(g => g.Organizer)
                    .Take(3)
                    .Select(g => (g.Organizer, g.Name))
                    .ToList()
            };
        }

        public static IEnumerable<MeetupStats> GetMeetupsStatistics(Meetup.DB meetups)
        {
            var meetupsStats = meetups
                .Groups.Where(group => group.Events.Length > 0)
                .OrderBy(group => group.Name)
                .Select(group =>
                {
                    var latestEvent = group.Events.Aggregate(
                        Meetup.Event.Null,
                        (prev, next) => next.When > prev.When ? next : prev
                    );
                    return new MeetupStats
                    {
                        Name = group.Name,
                        AverageAttendees = group.Events.Average(@event => @event.Attendees),
                        LatestEventTitle = latestEvent.Title,
                        LatestEventDate = latestEvent.When,
                        EverHadAtLeast200Attendees = group.Events.Any(@event =>
                            @event.Attendees >= 200
                        ),
                    };
                });

            return meetupsStats;
        }

        public static List<(string Name, string Title, string Url)> QueryArchitectsRepos(
            Meetup.DB meetups
        )
        {
            var architectsRepos = meetups
                .Groups.SelectMany(g => g.Events)
                .SelectMany(ev => ev.Agenda)
                .SelectMany(agenda => agenda.Speakers)
                .SelectMany(
                    speaker => speaker.Links,
                    (Meetup.Speaker speaker, Meetup.Link link) => (speaker, link)
                )
                .Where(speakerLink =>
                    speakerLink.speaker.Title.Contains("Architect")
                    && speakerLink.link.Title == "GitHub"
                )
                .GroupBy(speakerLink => speakerLink.speaker.Name, (name, items) => items.First())
                .OrderBy(speakerLink => speakerLink.speaker.Name)
                .Select(speakerLink =>
                    (speakerLink.speaker.Name, speakerLink.speaker.Title, speakerLink.link.Url)
                )
                .ToList();

            return architectsRepos;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Exercise 5A");
            Console.WriteLine();

            var meetups = Meetup.DB.Load();

            // -----------------
            // Challenge 1
            // -----------------
            var challenge1 = Challenge1(meetups);
            Console.WriteLine("Challenge 1: LINQ Basics");
            Console.WriteLine("------------------------");
            Console.WriteLine(
                $"Group names Upper Case: {string.Join(", ", challenge1.GroupNamesUpperCase)}"
            );
            Console.WriteLine($"Israeli groups: {string.Join(", ", challenge1.IsraeliGroups)}");
            Console.WriteLine(
                $"Number of Israeli groups: {string.Join(", ", challenge1.IsraeliGroupsCount)}"
            );
            Console.WriteLine($"Total events: {challenge1.TotalEvents}");
            Console.WriteLine(
                $"First 3 organizers: {string.Join(", ", challenge1.First3Organizers)}"
            );

            // -----------------
            // Challenge 2
            // -----------------
            var meetupsStats = GetMeetupsStatistics(meetups);

            Console.WriteLine("Challenge 2: Meetups Statistics");
            Console.WriteLine("-------------------------------");
            foreach (var stat in meetupsStats)
            {
                Console.WriteLine(stat.Name);
                Console.WriteLine($" - Average Attendees: {stat.AverageAttendees}");
                Console.WriteLine(
                    $" - Latest Event: {stat.LatestEventTitle} ({stat.LatestEventDate:Y})"
                );
                Console.WriteLine(
                    $" - 200 Attendees Badge: {(stat.EverHadAtLeast200Attendees ? " :) " : " --- ")}"
                );
                Console.WriteLine();
            }

            Console.WriteLine();

            // -----------------
            // Bonus Challenge 3
            // -----------------
            var architectsRepos = QueryArchitectsRepos(meetups);
            Console.WriteLine("Bonus Challenge 3: MVP Speakers");
            Console.WriteLine("--------------------------------");
            foreach (var item in architectsRepos)
            {
                Console.WriteLine(item.Name);
                Console.WriteLine($"  {item.Title}");
                Console.WriteLine($"  [{item.Url}]");
                Console.WriteLine();
            }

            Console.ReadLine();
        }
    }

    namespace Meetup
    {
        [DataContract]
        public class DB
        {
            public static DB Load()
            {
                using (FileStream openStream = File.OpenRead("Meetups.json"))
                {
                    var s = new System.Runtime.Serialization.Json.DataContractJsonSerializer(
                        typeof(DB)
                    );
                    var meetups = (DB)s.ReadObject(openStream);
                    return meetups;
                }
            }

            [DataMember(Name = "groups")]
            public Group[] Groups { get; set; } = new Group[0];
        }

        [DataContract]
        public class Group
        {
            public static readonly Group Null = new Group();

            [DataMember(Name = "name")]
            public string Name { get; set; } = string.Empty;

            [DataMember(Name = "organizer")]
            public string Organizer { get; set; } = string.Empty;

            [DataMember(Name = "events")]
            public Event[] Events { get; set; } = new Event[0];
        }

        [DataContract]
        public class Event
        {
            public static readonly Event Null = new Event();

            [DataMember(Name = "title")]
            public string Title { get; set; } = string.Empty;

            [DataMember(Name = "when")]
            private string _whenAsString { get; set; } = DateTime.MinValue.ToString("o");

            [IgnoreDataMember]
            public DateTime When
            {
                // Replace "o" with whichever DateTime format specifier you need.
                // "o" gives you a YYYY-MM-DDTHH:mm:SS.fff (ISO-8601).
                get { return DateTime.Parse(_whenAsString); }
                set { _whenAsString = value.ToString("o"); }
            }

            [DataMember(Name = "where")]
            public string Where { get; set; } = string.Empty;

            [DataMember(Name = "agenda")]
            public Agenda[] Agenda { get; set; } = new Agenda[0];

            [DataMember(Name = "attendees")]
            public int Attendees { get; set; }
        }

        [DataContract]
        public class Agenda
        {
            public static readonly Agenda Null = new Agenda();

            [DataMember(Name = "title")]
            public string Title { get; set; } = string.Empty;

            [DataMember(Name = "startTime")]
            public string StartTime { get; set; } = string.Empty;

            [DataMember(Name = "endTime")]
            public string EndTime { get; set; } = string.Empty;

            [DataMember(Name = "speakers")]
            public Speaker[] Speakers { get; set; } = new Speaker[0];
        }

        [DataContract]
        public class Speaker
        {
            public static readonly Speaker Null = new Speaker();

            [DataMember(Name = "name")]
            public string Name { get; set; } = string.Empty;

            [DataMember(Name = "title")]
            public string Title { get; set; } = string.Empty;

            [DataMember(Name = "links")]
            public Link[] Links { get; set; } = new Link[0];
        }

        [DataContract]
        public class Link
        {
            public static readonly Link Null = new Link();

            [DataMember(Name = "title")]
            public string Title { get; set; } = string.Empty;

            [DataMember(Name = "url")]
            public string Url { get; set; } = string.Empty;
        }
    }
}
