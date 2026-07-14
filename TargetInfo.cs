namespace SalvatalonMud
{
    internal class TargetInfo
    {
        public string Name { get; }
        public int Index { get; }

        public TargetInfo(string name, int index)
        {
            Name = name;
            Index = index;
        }
    }
}
