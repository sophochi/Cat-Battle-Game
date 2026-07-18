using System;
using System.Linq;

namespace ConsoleApp1
{
    public interface IShopState
    {
        void OpenShop();
    }


    public class EarlyGameShopState : IShopState
    {
        private Shop _shop;

        public EarlyGameShopState(Shop shop)
        {
            _shop = shop;
        }

        public void OpenShop()
        {
            string choice = "";

            while (choice != "0")
            {
                Console.WriteLine("\nSHOP: LEVEL 1-2 GOODS");

                Console.WriteLine($"You have: {_shop.Economy.Crystals} crystals\n");
                Console.WriteLine("1 — Small Snack (2 crystals) (Heals 3 HP)");
                Console.WriteLine("2 — Magic Bell (3 crystals) (Item)");
                Console.WriteLine("0 — Exit shop");

                Console.Write("\nChoose item: ");
                choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": _shop.Buy("Small Snack", 2); break;
                    case "2": _shop.Buy("Magic Bell", 3); break;
                    case "0": Console.WriteLine("Leaving shop..."); break;
                    default: Console.WriteLine("Wrong option. Try again."); break;
                }
            }
        }
    }

    public class LateGameShopState : IShopState
        {
            private Shop _shop;

            public LateGameShopState(Shop shop)
            {
                _shop = shop;
            }

            public void OpenShop()
            {
                string choice = "";

                while (choice != "0")
                {
                    Console.WriteLine("\nSHOP: END-GAME SUPPLIES");
                    Console.WriteLine($"You have: {_shop.Economy.Crystals} crystals\n");

                    Console.WriteLine("1 — Mega Bomb (15 crystals) (Massive Damage)");
                    Console.WriteLine("2 — Super Shield (20 crystals) (Defence Item)");
                    Console.WriteLine("0 — Exit shop");

                    Console.Write("\nChoose item: ");
                    choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1": _shop.Buy("Mega Bomb", 15); break;
                        case "2": _shop.Buy("Super Shield", 20); break;
                        case "0": Console.WriteLine("Leaving shop..."); break;
                        default: Console.WriteLine("Wrong option. Try again."); break;
                    }
                }
            }
        }


        public static class ShopStateFactory
        {
            public static IShopState CreateStateForLevel(Shop shop, int levelID)
            {
                if (levelID <= 2) return new EarlyGameShopState(shop);
                else return new LateGameShopState(shop);
            }
        }

        public class Shop : IEconomyObserver
        {
            private Economy _economy;
            private IShopState _currentState;

            public Economy Economy
            {
                get { return _economy; }
            }

            public Shop(Economy economy)
            {
                _economy = economy;
                _economy.Subscribe(this);
                SetState(new EarlyGameShopState(this));
            }

            public void SetState(IShopState newState)
            {
                _currentState = newState;
            }

            public void Open()
            {
                _currentState.OpenShop();
            }

            public void Buy(string itemName, int price)
            {
                if (_economy.Crystals < price)
                {
                    Console.WriteLine("Not enough crystals!");
                    return;
                }

                _economy.Crystals -= price;
                Console.WriteLine($"You bought: {itemName}! Crystals left: {_economy.Crystals}");
            }

            public void Update(int newCrystalCount)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[SHOP INFO]");
                Console.WriteLine($"Balance refreshed! New crystal count: {newCrystalCount}");
                Console.ResetColor();
            }
        }
}
