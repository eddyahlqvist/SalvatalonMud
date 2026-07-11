using System.Collections.Generic;

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

        public string GetExitShort()
        {
            List<string> exits = new();

            if (North != null) exits.Add("n");
            if (South != null) exits.Add("s");
            if (East != null) exits.Add("e");
            if (West != null) exits.Add("w");

            return $"Exits: [{string.Join(", ", exits)}]";
        }
    }
}
