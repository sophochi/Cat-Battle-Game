using System;

namespace ConsoleApp1
{
    public class Item
    {
        public string Name { get; protected set; }
        public string Type { get; protected set; } // "Resource", "Weapon", "Food", etc.
        public int Value { get; protected set; }

        public Item(string name, string type, int value)
        {
            Name = name;
            Type = type;
            Value = value;
        }
    }
}
