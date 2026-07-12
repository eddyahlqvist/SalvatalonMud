using System.Collections.Generic;

namespace SalvatalonMud
{
    internal class World
    {
        public string Name { get; }
        public Room StartingRoom { get; }
        public IReadOnlyList<Room> Rooms { get; }
        public World(string name, Room startingRoom, IReadOnlyList<Room> rooms)
        {
            Name = name;
            StartingRoom = startingRoom;
            Rooms = rooms;
        }
    }
}
