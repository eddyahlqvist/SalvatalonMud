namespace SalvatalonMud
{
    internal class PlayerBuilder
    {
        public Player Build(string name, Room startingRoom, Race race, bool isImmortal)
        {
            Stats stats = new Stats(
                race.StartingStrength,
                race.StartingDexterity,
                race.StartingConstitution,
                race.StartingIntelligence,
                race.StartingWisdom
            );

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
