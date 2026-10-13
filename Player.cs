using System.Collections.Generic;

namespace SalvatalonMud
{
    internal class Player
    {
        public string Name { get; }
        public Race Race { get; }
        public Room CurrentRoom { get; set; }
        public bool IsImmortal { get; }
        public List<Item> Inventory { get; } = new();
        public CoinPurse CoinPurse { get; } = new();
        public Stats Stats { get; }
        public int MaxCarryWeight => Stats.Strength * 10;
        private const int ConstitutionModifier = 10;
        public int HealthPoints { get; set; }
        public int MaxHealthPoints => Stats.Constitution * ConstitutionModifier;

        public Player(
            string name,
            Room currentRoom,
            Race race,
            bool isImmortal,
            Stats stats
            )
        {
            Name = name;
            CurrentRoom = currentRoom;
            Race = race;
            IsImmortal = isImmortal;
            Stats = stats;

            HealthPoints = MaxHealthPoints;
        }

        public void MoveTo(Room destination)
        {
            CurrentRoom.Players.Remove(this);

            CurrentRoom = destination;

            destination.Players.Add(this);
        }

        public int GetInventoryWeight()
        {
            int totalWeight = 0;

            foreach (Item item in Inventory)
            {
                totalWeight += item.Weight;
            }

            return totalWeight;
        }
    }
}
