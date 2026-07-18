using System;

namespace ConsoleApp1
{
    public class GameManager
    {
        private Map _map;
        private string _playerNick;

        private Cat _playerCat;
        private EnemyGroup _enemyGroup;
        private Economy _economy;
        private Shop _shop;

        public void StartGame()
        {
            RunIntro();
            ChoosePlayerCat();

            RunMap();
            PrepareLevelFight(1);
        }

        public void RunIntro()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("- hmm... where am I?");
            Console.ResetColor();

            Console.WriteLine("Welcome to CatLand, our Savior! We are happy to see you here! (press Enter to continue)");
            Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("- 'our Savior'? What does it even mean? Am I dreaming?");
            Console.ResetColor();

            Console.WriteLine("This world is real and it's true... Only you can save our cats \n You were destined to help kittens. " +
                "Your spirit is strong and will for adventures is even stronger! (press Enter to continue)");
            Console.ReadLine();

            Console.WriteLine("You see that we are standing on the ruins... Wanna hear the story? (press Y to confirm)");
            string answ = Console.ReadLine();

            if (answ == "y")
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("- yeah, I'm all ears");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("huh? no? but I'll still tell you. So listen,");
            }

            Console.WriteLine("In a world once filled with endless joy, fresh catnip, " +
                "and colorful balls of yarn, cats ruled in peace and freedom.\n" +
                "But one day, the portal between worlds opened - and chaos followed. " +
                " The invaders came: Garbage (dead russians), Vacuum Robots, Overgrown Mice," +
                " Cucumber Mutants, and the most terrifying of all… Crazy Granny. \n" +
                "\nNow cats are enslaved, forced to bathe, and stripped of their independence.\n" +
                "But not all hope is lost. Deep within you burns the spirit of the true cat warrior.\n" +
                "Find your courage, sharpen your claws, and take back your freedom.\n" +
                "May the whiskers guide your path - and may luck always be on your side.\n" +
                " (press Enter to continue)");
            Console.ReadLine();

            Console.WriteLine("Fearless warrior, speak your name, that it may echo through the ages and be remembered forever:");
            _playerNick = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"- {_playerNick}");
            Console.ResetColor();
        }

        private void ChoosePlayerCat()
        {
            Console.WriteLine("\nChoose your cat class:");
            Console.WriteLine("1 — Ninja (Fast attacker)");
            Console.WriteLine("2 — Tank (High defense & HP)");
            Console.WriteLine("3 — Healer (Can heal allies)");
            Console.WriteLine("4 — Berserk (Rage mode)");

            string choice = Console.ReadLine();

            Console.WriteLine("Give your cat a name:");
            string catName = Console.ReadLine();

            Cat chosenCat = null;
            int startLevel = 1;
            int startStars = 3; // стартова кількість зірок для гравця

            switch (choice)
            {
                case "1":
                    chosenCat = new Ninja(catName, startLevel, startStars);
                    break;
                case "2":
                    chosenCat = new Tank(catName, startLevel, startStars);
                    break;
                case "3":
                    chosenCat = new Healer(catName, startLevel, startStars);
                    break;
                case "4":
                    chosenCat = new Berserk(catName, startLevel, startStars);
                    break;
                default:
                    Console.WriteLine("Invalid option! Your cat is going to be a ninja.");
                    chosenCat = new Ninja(catName, startLevel, startStars);
                    break;
            }

            _playerCat = chosenCat;

            Console.WriteLine("\nYour chosen cat:");
            _playerCat.ShowCardInfo();
        }

        private void RunMap()
        {
            Console.WriteLine($"{_playerNick}, you are now on the map...");
            
            _economy = new Economy(10);
            _shop = new Shop(_economy);

            _map = new Map(_economy, _shop);

            Console.WriteLine("Start level 1? (press Y)");
            string ans = Console.ReadLine().ToLower();

            while (ans != "y")
            {
                Console.WriteLine("You must save the cats! Press Y!");
                ans = Console.ReadLine().ToLower();
            }

            Console.WriteLine("Visit shop before fight? (y/n)");
            if (Console.ReadLine().ToLower() == "y")
            {
                _map.EnterShop();
            }

            _map.SelectedLevel(1);
            _map.EnterLevel();
        }

        private void PrepareLevelFight(int levelID)
        {
            Level level = Levels.FindByID(levelID);

            if (level == null || !Levels.IsUnlocked(level))
            {
                Console.WriteLine("Level is locked.");
                return;
            }
            
            IShopState newState = ShopStateFactory.CreateStateForLevel(_shop, levelID);
            _shop.SetState(newState);
            
            Console.WriteLine($"Preparing fight for level {level.Name}...");

            Console.WriteLine("Your cat warrior today:");
            _playerCat.ShowCardInfo();

            _enemyGroup = new EnemyGroup(level.Name + " Enemies");
            for (int i = 0; i < level.EnemyCount; i++)
            {
                _enemyGroup.AddEnemy(CreateEnemy(level.enemyType));
            }

            _enemyGroup.PrintStatus();

            StartBattle(level);
        }

        private Enemy CreateEnemy(EnemyType type)
        {
            switch (type)
            {
                case EnemyType.Garbage: return new Garbage();
                case EnemyType.RobotVacuum: return new RobotVacuum();
                case EnemyType.MuscleMouse: return new MuscleMouse();
                case EnemyType.MutantCucumber: return new MutantCucumber();
                case EnemyType.WildGranny: return new WildGranny();
                default: return new Garbage();
            }
        }

        private void StartBattle(Level level)
        {
            Console.WriteLine("\nBATTLE START");

            while (_playerCat.IsAlive && level.EnemyCount > 0)
            {
                _playerCat.OnTurnStart();
                _enemyGroup.ApplyTurnStart();

                if (!_playerCat.IsAlive) break;

                PlayerTurn(level);

                if (level.EnemyCount <= 0) break;
                if (!_playerCat.IsAlive) break;

                EnemyTurn();
            }

            EndBattle(level);
        }

        private void PlayerTurn(Level level)
        {
            Console.WriteLine("\nPlayer Turn <-");

            Enemy target = _enemyGroup.Enemies.Find(e => e.IsAlive);
            if (target == null) return;

            _playerCat.AttackTarget(target);

            if (!target.IsAlive)
            {
                level.RegisterEnemyDefeated();
                _enemyGroup.RemoveDead();
            }
        }

        private void EnemyTurn()
        {
            Console.WriteLine("\nEnemy Turn ->");
            _enemyGroup.GroupAttack(_playerCat, _economy);
        }

        private void EndBattle(Level level)
        {
            Console.WriteLine("\nBATTLE FINISHED");

            if (Levels.IsVictory(level))
            {
                Console.WriteLine("Your cats won the fight!");
                Levels.CompletedLevel(level);

                int nextLevelID = level.ID + 1;
                IShopState nextState = ShopStateFactory.CreateStateForLevel(_shop, nextLevelID);
                _shop.SetState(nextState);
            }
            else
            {
                Console.WriteLine("Your cats lost the battle...");
            }

            _map.EndLevel();
        }
    }
}
