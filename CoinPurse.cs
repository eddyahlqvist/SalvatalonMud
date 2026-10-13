namespace SalvatalonMud
{
    internal class CoinPurse
    {
        public long Brass { get; set; }
        public long Copper { get; set; }
        public long Silver { get; set; }
        public long Gold { get; set; }
        public long Platinum { get; set; }
        public long TotalValue =>
            Brass +
            Copper * 10 +
            Silver * 100 +
            Gold * 1000 +
            Platinum * 5000;
    }
}
