namespace SalvatalonMud
{
    internal class Player
    {
        public string Name { get; }        
        public Room CurrentRoom { get; set; }
        public int HealthPoints { get; set; }

        public Player(string name, Room currentRoom, int healthPoints)
        {
            Name = name;
            CurrentRoom = currentRoom;
            HealthPoints = healthPoints;            
        }
    }
}
