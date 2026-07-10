using System.Collections.Generic;

namespace SalvatalonMud
{
    internal class World
    {
        public List<Room> Rooms { get; }
        public World(List<Room> rooms)
        {
            Rooms = rooms;
        }
    }
}
