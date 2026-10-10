using System.Collections.Generic;

namespace SalvatalonMud
{
    internal class Player
    {
        public string Name { get; }
        public Race Race { get; }
        public Room CurrentRoom { get; set; }
        public int HealthPoints { get; set; }
        public List<Item> Inventory { get; } = new();
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Constitution { get; set; }
        public int Intelligence { get; set; }
        public int Wisdom { get; set; }
        public int MaxCarryWeight => Strength * 10;
        private const int ConstitutionModifier = 10;
        public int MaxHealthPoints => Constitution * ConstitutionModifier;



        public Player(
            string name,
            Room currentRoom,
            Race race
            )
        {
            Name = name;
            CurrentRoom = currentRoom;
            Race = race;

            Strength = race.StartingStrength;
            Dexterity = race.StartingDexterity;
            Constitution = race.StartingConstitution;
            Intelligence = race.StartingIntelligence;
            Wisdom = race.StartingWisdom;

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
