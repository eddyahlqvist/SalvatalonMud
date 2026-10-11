namespace SalvatalonMud
{
    internal class Stats
    {
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Constitution { get; set; }
        public int Intelligence { get; set; }
        public int Wisdom { get; set; }

        public Stats(
           int startingStrength,
           int startingDexterity,
           int startingConstitution,
           int startingIntelligence,
           int startingWisdom)
        {
            Strength = startingStrength;
            Dexterity = startingDexterity;
            Constitution = startingConstitution;
            Intelligence = startingIntelligence;
            Wisdom = startingWisdom;
        }
    }
}