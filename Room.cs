namespace SalvatalonMud
{
    internal class Room
    {
        public string Name { get; }
        public string Description { get; }      
        
        public Room? North { get; set; }
        public Room? South { get; set; }
        public Room? East { get; set; }
        public Room? West { get; set; }

        public Room(string name, string description)
        {
            Name = name;
            Description = description;
        }        
    }
}
