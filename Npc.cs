namespace SalvatalonMud
{
    internal class Npc
    {
        public string Name { get; }
        public string Description { get; }
        public Room CurrentRoom { get; set; }
        public int HealthPoints { get; set; }

        public Npc(
            string name,
            string description,
            Room currentRoom,
            int healthPoints)
        {
            Name = name;
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
