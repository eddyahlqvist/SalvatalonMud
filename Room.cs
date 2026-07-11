using System.Collections.Generic;

namespace SalvatalonMud
{
    internal class Room
    {
        public string Name { get; }
        public string Description { get; }

        // Coordinates (Cartesian grid-style)
        // tyrikaSquare = X:0, Y:0, Z:0 (Z:0 is sea level)
        public int X { get; }
        public int Y { get; }
        public int Z { get; }

        public Room? North { get; set; }
        public Room? South { get; set; }
        public Room? East { get; set; }
        public Room? West { get; set; }

        public Room(string name, string description, int x, int y, int z)
        {
            Name = name;
            Description = description;

            X = x;
            Y = y;
            Z = z;
        }

        public string GetExitShort()
        {
            List<string> exits = new();

            if (North != null) exits.Add("n");
            if (South != null) exits.Add("s");
            if (East != null) exits.Add("e");
            if (West != null) exits.Add("w");

            return $"Exits: [{string.Join(", ", exits)}]";
        }
    }
}
