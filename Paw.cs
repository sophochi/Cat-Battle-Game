using System;

namespace ConsoleApp1
{
    public class Paw
    {
        public Item HeldItem { get; protected set; }
        public bool IsFree
        {
            get { return HeldItem == null; }
        }

        public Paw()
        {
            HeldItem = null;
        }

        public void TakeItem(Item item)
        {
            if (!IsFree)
            {
                Console.WriteLine($"Cat already holds {HeldItem.Name}");
                return;
            }

            HeldItem = item;
            Console.WriteLine($"Cat took {item.Name}");
        }

        public Item DropItem()
        {
            if (IsFree)
            {
                Console.WriteLine($"Cat don't hold anything");
                return null;
            }

            Item dropped = HeldItem;
            HeldItem = null;
            Console.WriteLine($"Cat dropped {dropped.Name}");
            return dropped;

        }
    }
}
