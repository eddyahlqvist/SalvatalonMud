using System.Collections.Generic;
using System.Globalization;

namespace SalvatalonMud
{
    internal class Npc
    {
        public string Name { get; }
        public List<string> Keywords { get; }
        public string DisplayName { get; }
        public string DisplayNamePlural { get; }
        public string Description { get; }
        public Room? CurrentRoom { get; set; }
        public List<Item> Inventory { get; } = new();
        public Stats Stats { get; }
        private const int ConstitutionModifier = 10;
        public int HealthPoints { get; set; }
        public int MaxHealthPoints => Stats.Constitution * ConstitutionModifier;

        public Npc(
            string name,
            string displayNamePlural,
            IEnumerable<string> keywords,
            string description,
            Stats stats)
        {
            Name = name;

            DisplayName = CultureInfo
                .InvariantCulture
                .TextInfo
                .ToTitleCase(name);

            DisplayNamePlural = displayNamePlural;
            Keywords = new List<string>(keywords);
            Description = description;
            CurrentRoom = null;
            Stats = stats;
            HealthPoints = MaxHealthPoints;
        }

        public bool Matches(string input)
        {
            return Name == input || Keywords.Contains(input);
        }

        public void MoveTo(Room destination)
        {
            CurrentRoom!.Npcs.Remove(this);

            CurrentRoom = destination;

            destination.Npcs.Add(this);
        }
    }
}
