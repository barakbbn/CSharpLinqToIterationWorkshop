using System;
using System.IO;
using System.Linq;
using System.Security.Policy;
using NUnit.Framework;

namespace Exercise_5A
{
    [TestFixture]
    public class Challenge1Tests
    {
        [Test]
        public void GroupNamesUpperCase()
        {
            var meetups = Meetup.DB.Load();
            var expected = new string[]
            {
                "ALT.NET ISRAEL",
                "NODE.JS ISRAEL",
                "JAVASCRIPT ISRAEL",
                "LIFE MICHAEL",
                "CODECRAFT IL",
                "MY NEW MEETUP GROUP IL"
            };
            var actual = Program.Challenge1(meetups);
            Assert.That(actual.GroupNamesUpperCase, Is.EqualTo(expected));
        }

        [Test]
        public void IsraeliGroups()
        {
            var meetups = Meetup.DB.Load();
            var expected = new string[] { "ALT.NET Israel", "Node.js Israel", "JavaScript Israel" };
            var actual = Program.Challenge1(meetups);
            Assert.That(actual.IsraeliGroups, Is.EqualTo(expected));
        }

        [Test]
        public void IsraeliGroupsCount()
        {
            var meetups = Meetup.DB.Load();
            var expected = 3;
            var actual = Program.Challenge1(meetups);
            Assert.AreEqual(expected, actual.IsraeliGroupsCount);
        }

        [Test]
        public void TotalEvents()
        {
            var meetups = Meetup.DB.Load();
            var expected = 27;
            var actual = Program.Challenge1(meetups);
            Assert.AreEqual(expected, actual.TotalEvents);
        }

        [Test]
        public void First3Organizers()
        {
            var meetups = Meetup.DB.Load();
            var expected = new[]
            {
                ("Haim Michael", "life michael"),
                ("Lior Kesos", "Node.js Israel"),
                ("Me", "My New Meetup Group IL")
            };
            var actual = Program.Challenge1(meetups);
            Assert.That(actual.First3Organizers, Is.EqualTo(expected));
        }
    }

    [TestFixture]
    public class MeetupTests
    {
        [Test]
        public void GetMeetupsStatistics()
        {
            var expected = new MeetupStats[]
            {
                new MeetupStats
                {
                    Name = "ALT.NET Israel",
                    EventsCount = 4,
                    AverageAttendees = 57,
                    LatestEventTitle = "ALT.NET Night - March 2023",
                    LatestEventDate = DateTime.Parse("2023-05-01T18:00:00+02:00"),
                    EverHadAtLeast200Attendees = false
                },
                new MeetupStats
                {
                    Name = "Codecraft IL",
                    EventsCount = 5,
                    AverageAttendees = 114.6,
                    LatestEventTitle = "Clean Code Principles#2",
                    LatestEventDate = DateTime.Parse("2022-05-29T11:00:00+03:00"),
                    EverHadAtLeast200Attendees = false
                },
                new MeetupStats
                {
                    Name = "JavaScript Israel",
                    EventsCount = 6,
                    AverageAttendees = 217.5,
                    LatestEventTitle = "Let's Go Headless - A Dive into Headless Components",
                    LatestEventDate = DateTime.Parse("2023-08-20T18:30:00+03:00"),
                    EverHadAtLeast200Attendees = true
                },
                new MeetupStats
                {
                    Name = "life michael",
                    EventsCount = 7,
                    AverageAttendees = 78,
                    LatestEventTitle = "Anti Patterns [Free Meetup]",
                    LatestEventDate = DateTime.Parse("2023-08-01T13:00:00+03:00"),
                    EverHadAtLeast200Attendees = false
                },
                new MeetupStats
                {
                    Name = "Node.js Israel",
                    EventsCount = 5,
                    AverageAttendees = 282.4,
                    LatestEventTitle = "May 2023 Node.js Monthly Meetup",
                    LatestEventDate = DateTime.Parse("2023-05-29T18:00:00+03:00"),
                    EverHadAtLeast200Attendees = true
                },
            };

            var meetups = Meetup.DB.Load();
            var actual = Program.GetMeetupsStatistics(meetups).ToList();

            if (actual.Count != expected.Length)
            {
                throw new InvalidDataException(
                    $"Expected to find {expected.Length} results, but actually has {actual.Count}"
                );
            }
            for (var i = 0; i < actual.Count; i++)
            {
                var act = actual[i];
                var exp = expected[i];
                if (!act.Equals(exp))
                    throw new InvalidDataException(
                        $"Actual item [{i}] not same as expected:\n  Actual: {act}\nExpected: {exp}"
                    );
            }
        }

        [Test]
        public void QueryArchitectsRepos()
        {
            var expected = new (string Name, string Title, string Url)[]
            {
                (
                    "Alon Valadji",
                    "Senior web technologies consultant and Software Architect",
                    "https://github.com/alonronin"
                ),
                ("Avishay Maor", "Fullstack Architect & Group Lead", "https://github.com/mavishay"),
                ("Gil Tayar", "Senior Software Architect", "https://github.com/giltayar"),
                ("Yonatan Kra", "Software Architect", "https://github.com/YonatanKra"),
            };

            var meetups = Meetup.DB.Load();
            var actual = Program.QueryArchitectsRepos(meetups);

            if (actual.Count != expected.Length)
            {
                throw new InvalidDataException(
                    $"Expected to find {expected.Length} results, but actually has {actual.Count}"
                );
            }
            for (var i = 0; i < actual.Count; i++)
            {
                var act = actual[i];
                var exp = expected[i];
                if (!act.Equals(exp))
                    throw new InvalidDataException(
                        $"Actual item [{i}] not same as expected:\n  Actual: {act}\nExpected: {exp}"
                    );
            }
        }
    }
}
