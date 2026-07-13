using System.Collections.Generic;
using System.Globalization;

namespace SalvatalonMud
{
    internal class Npc
    {
        public string Name { get; }
        public List<string> Keywords { get; }
        public string DisplayName { get; }
        public string Description { get; }
        public Room CurrentRoom { get; set; }
        public int HealthPoints { get; set; }

        public Npc(
            string name,
            IEnumerable<string> keywords,
            string description,            
            Room currentRoom,
            int healthPoints)
        {
            Name = name;

            DisplayName = CultureInfo
                .InvariantCulture
                .TextInfo
                .ToTitleCase(name);

            Keywords = new List<string>(keywords);
            Description = description;
            CurrentRoom = currentRoom;
            HealthPoints = healthPoints;
        }

        public bool Matches(string input)
        {
            return Name == input || Keywords.Contains(input);
        }

        public void MoveTo(Room destination)
        {
            CurrentRoom.Npcs.Remove(this);

            CurrentRoom = destination;

            destination.Npcs.Add(this);
        }
    }
}
