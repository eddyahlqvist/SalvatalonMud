namespace SalvatalonMud
{
    internal class PlayerBuilder
    {
        public Player Build(string name, Room startingRoom, Race race, bool isImmortal, Stats stats)
        {
            return new Player(
                name: name,
                currentRoom: startingRoom,
                race: race,
                isImmortal: isImmortal,
                stats: stats
                );
        }
    }
}
