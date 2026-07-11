namespace SalvatalonMud
{
    internal class PlayerBuilder
    {
        public Player Build(string name, Room startingRoom)
        {
            const int StartingHealthPoints = 10;
            return new Player(name, startingRoom, StartingHealthPoints);
        }
    }
}
