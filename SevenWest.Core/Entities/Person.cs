namespace SevenWest.Core.Entities
{
    public class Person
    {
        public int Id { get; set; }
        public string First { get; set; }
        public string Last { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }

        public string FullName => $"{First} {Last}";
    }
}
