using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    public class Inventory
    {
        public int Capacity { get; protected set; }
        public List<Item> Items { get; protected set; }

        public Inventory(int capacity)
        {
            Capacity = capacity;
            Items = new List<Item>();
        }

        public void ShowContents()
        {
            Console.WriteLine("Inventory contents:");
            if (Items.Count == 0)
            {
                Console.WriteLine(" empty.");
            }

            for (int i = 0; i < Items.Count; i++)
            {
                Console.WriteLine($" {i + 1}. {Items[i].Name}");
            }
        }

        public void AddItem(Item item)
        {
            if (Items.Count >= Capacity)
            {
                Console.WriteLine("There are no space in the inventory");
                return;
            }

            Items.Add(item);
            Console.WriteLine($"{item.Name} was put in the inventory");
        }

        public void RemoveItemByIndex(int index)
        {
            if (index < 0 || index >= Items.Count)
            {
                Console.WriteLine("Wrong index");
                return;
            }

            Console.WriteLine($"{Items[index].Name} was removed from inventory");
            Items.RemoveAt(index);
        }

        public void OpenBackpack()
        {
            while (true)
            {
                ShowContents();

                Console.WriteLine("Write the number of the item you want to remove (or 0 to exit): ");
                string input = Console.ReadLine();

                int choice;
                if (!int.TryParse(input, out choice))
                {
                    Console.WriteLine("Write a number!");
                    continue;
                }

                if (choice == 0)
                {
                    Console.WriteLine("Inventory is closed");
                    break;
                }

                RemoveItemByIndex(choice - 1);
            }
        }

    }
}
