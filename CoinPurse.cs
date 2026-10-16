using System;

namespace SalvatalonMud
{
    internal class CoinPurse
    {
        private const int CopperValue = 10;
        private const int SilverValue = 100;
        private const int GoldValue = 1000;
        private const int PlatinumValue = 5000;

        public long Brass { get; private set; }
        public long Copper { get; private set; }
        public long Silver { get; private set; }
        public long Gold { get; private set; }
        public long Platinum { get; private set; }
        public long TotalValue => checked(
            Brass +
            Copper * CopperValue +
            Silver * SilverValue +
            Gold * GoldValue +
            Platinum * PlatinumValue);

        public void SetDenomination(Denomination denomination, long amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount));
            }

            switch (denomination)
            {
                case Denomination.Brass:
                    Brass = amount;
                    break;
                case Denomination.Copper:
                    Copper = amount;
                    break;
                case Denomination.Silver:
                    Silver = amount;
                    break;
                case Denomination.Gold:
                    Gold = amount;
                    break;
                case Denomination.Platinum:
                    Platinum = amount;
                    break;
                default:
                    throw new ArgumentException("Invalid denomination", nameof(denomination));
            }
        }

        public void AddDenomination(Denomination denomination, long amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount));
            }

            checked
            {
                switch (denomination)
                {
                    case Denomination.Brass:
                        Brass += amount;
                        break;
                    case Denomination.Copper:
                        Copper += amount;
                        break;
                    case Denomination.Silver:
                        Silver += amount;
                        break;
                    case Denomination.Gold:
                        Gold += amount;
                        break;
                    case Denomination.Platinum:
                        Platinum += amount;
                        break;
                    default:
                        throw new ArgumentException("Invalid denomination", nameof(denomination));
                }
            }
        }

        public bool TryRemoveDenomination(Denomination denomination, long amount)
        {
            if (amount <= 0)
            {
                return false;
            }

            switch (denomination)
            {
                case Denomination.Brass:
                    if (Brass < amount)
                        return false;

                    Brass -= amount;
                    return true;
                case Denomination.Copper:
                    if (Copper < amount)
                        return false;

                    Copper -= amount;
                    return true;
                case Denomination.Silver:
                    if (Silver < amount)
                        return false;

                    Silver -= amount;
                    return true;
                case Denomination.Gold:
                    if (Gold < amount)
                        return false;

                    Gold -= amount;
                    return true;
                case Denomination.Platinum:
                    if (Platinum < amount)
                        return false;

                    Platinum -= amount;
                    return true;
                default:
                    throw new ArgumentException("Invalid denomination", nameof(denomination));
            }
        }
    }
}

