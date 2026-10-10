using System.Collections.Generic;
using System.Globalization;

namespace SalvatalonMud
{
    internal class Item
    {
        public string Name { get; }
        public string Description { get; }
        public int Weight { get; }
        public List<string> Keywords { get; }
        public string DisplayName { get; }
        public string DisplayNamePlural { get; }

        public Item(string name, string displayNamePlural, string description, int weight, IEnumerable<string> keywords)
        {
            Name = name;

            DisplayName = CultureInfo
                .InvariantCulture
                .TextInfo
                .ToTitleCase(name);

            DisplayNamePlural = displayNamePlural;
            Description = description;
            Weight = weight;
            Keywords = new List<string>(keywords);
        }

        public bool Matches(string input)
        {
            return Name == input || Keywords.Contains(input);
        }

        public void MoveToInventory(Player player)
        {
            player.CurrentRoom.Items.Remove(this);
            player.Inventory.Add(this);
        }

        public void MoveToRoom(Player player)
        {
            player.Inventory.Remove(this);
            player.CurrentRoom.Items.Add(this);
        }
    }
}
