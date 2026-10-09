namespace SalvatalonMud
{
    internal class Item
    {
        public string Name { get; }
        public string Description { get; }
        public int Weight { get; }       

        public Item(string name, string description, int weight)
        {
            Name = name;
            Description = description;
            Weight = weight;
        }
    }
}
