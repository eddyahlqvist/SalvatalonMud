using System.Globalization;

namespace SalvatalonMud
{
    internal class Npc
    {
        public string Name { get; }
        public string Tag { get; }
        public string DisplayName { get; }
        public string Description { get; }
        public Room CurrentRoom { get; set; }
        public int HealthPoints { get; set; }

        public Npc(
            string name,
            string tag,
            string description,            
            Room currentRoom,
            int healthPoints)
        {
            Name = name;
            DisplayName = CultureInfo
                .InvariantCulture
                .TextInfo
                .ToTitleCase(name);

            Tag = tag;
            Description = description;
            CurrentRoom = currentRoom;
            HealthPoints = healthPoints;
        }


        public void MoveTo(Room destination)
        {
            CurrentRoom.Npcs.Remove(this);

            CurrentRoom = destination;

            destination.Npcs.Add(this);
        }
    }
}
