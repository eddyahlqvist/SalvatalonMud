namespace SalvatalonMud
{
    internal class CoinPurse
    {
        private const int CopperValue = 10;
        private const int SilverValue = 100;
        private const int GoldValue = 1000;
        private const int PlatinumValue = 5000;

        public long Brass { get; set; }
        public long Copper { get; set; }
        public long Silver { get; set; }
        public long Gold { get; set; }
        public long Platinum { get; set; }
        public long TotalValue =>
            Brass +
            Copper * CopperValue +
            Silver * SilverValue +
            Gold * GoldValue +
            Platinum * PlatinumValue;
    }
}
