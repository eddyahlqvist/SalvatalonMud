namespace SalvatalonMud
{
    internal class Race
    {
        public string Name { get; }
        public int StartingStrength { get; }
        public int StartingDexterity { get; }
        public int StartingConstitution { get; }
        public int StartingIntelligence { get; }
        public int StartingWisdom { get; }

        public Race(
            string name,
            int startingStrength,
            int startingDexterity,
            int startingConstitution,
            int startingIntelligence,
            int startingWisdom)
        {
            Name = name;
            StartingStrength = startingStrength;
            StartingDexterity = startingDexterity;
            StartingConstitution = startingConstitution;
            StartingIntelligence = startingIntelligence;
            StartingWisdom = startingWisdom;
        }
    }
}
