using System.Collections.Generic;

namespace SalvatalonMud
{
    internal class World
    {
        public string Name { get; }
        public List<Room> Rooms { get; }
        public World(string name, List<Room> rooms)
        {
            Name = name;
            Rooms = rooms;
        }
    }
}
