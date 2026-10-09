using System.Collections.Generic;

namespace SalvatalonMud
{
    internal class Player
    {
        public string Name { get; }        
        public Room CurrentRoom { get; set; }
        public int HealthPoints { get; set; }
        public List<Item> Inventory { get; } = new();

        public Player(string name, Room currentRoom, int healthPoints)
        {
            Name = name;
            CurrentRoom = currentRoom;
            HealthPoints = healthPoints;            
        }

        public void MoveTo(Room destination)
        {
            CurrentRoom.Players.Remove(this);

            CurrentRoom = destination;

            destination.Players.Add(this);
        }
    }
}
