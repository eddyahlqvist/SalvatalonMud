namespace SalvatalonMud
{
    internal class PlayerBuilder
    {
        public Player Build(string name)
        {
            const int StartingHealthPoints = 10;
            return new Player(name, StartingHealthPoints);
        }
    }
}
