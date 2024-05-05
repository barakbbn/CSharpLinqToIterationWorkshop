namespace Demo6
{
    internal class Student
    {
        public Student()
        {
            Address = new Address();
        }

        public int Grade { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Address Address { get; private set; }
    }
    internal class Address
    {
        public string Country { get; set; }
        public string City { get; set; }
    }
}