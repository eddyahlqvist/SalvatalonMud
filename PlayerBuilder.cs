namespace SalvatalonMud
{
    internal class PlayerBuilder
    {
        public Player Build(string name, Room startingRoom, Race race)
        {
            return new Player(
                name: name,
                currentRoom: startingRoom,
                race: race);
        }
    }
}
