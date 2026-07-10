namespace SalvatalonMud
{
    internal class Player
    {
        public string Name { get; }        
        public int HealthPoints { get; set; }

        public Player(string name, int healthPoints)
        {
            Name = name;
            HealthPoints = healthPoints;            
        }
    }
}
