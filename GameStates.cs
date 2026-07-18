using System;

namespace ConsoleApp1
{
    public class StateOnMap : State
    {
        public StateOnMap(Map map) : base(map) { }

        public override void SelectedLevel(int levelID)
        {
            Level selected = Levels.FindByID(levelID);
            if (selected != null && Levels.IsUnlocked(selected))
            {
                map.currLevelID = levelID - 1;
                Console.WriteLine($"Selected level: {selected.ID}  - {selected.Name}");

                map.ChangeState(new StateViewingLevel(map));
            }
            else
            {
                Console.WriteLine("This level is locked. Please complete previous levels");
            }
        }

        public override void EnterShop()
        {
            map.ChangeState(new StateShop(map));
        }
    }

    public class StateViewingLevel : State
    {
        public StateViewingLevel(Map map) : base(map) { }

        public override void EnterLevel()
        {
            Console.WriteLine($"Starting {map.levels[map.currLevelID].Name} level");
            map.ChangeState(new StateInLevel(map));
        }
    }

    public class StateInLevel : State
    {
        public StateInLevel(Map map) : base(map) { }

        public override void EndLevel()
        {
             Level level = map.levels[map.currLevelID];

            if (Levels.IsVictory(level))
            {
                Console.WriteLine("Victory! Your cats have defeated the enemies and claimed all the snacks. Time for a celebratory nap!");
                Levels.CompletedLevel(level);

                map.gameEconomy.Crystals += 5;
                Console.WriteLine("Please take this little gift -- 5 crystals");
            }
            else
            {
                Console.WriteLine($"Defeat. Your noble cats have fallen…  {level.enemyType} now make them bathe. Rise again, commander.");
            }

            map.ChangeState(new StateOnMap(map));
            
        }
    }

    public class StateShop : State
    {
        public StateShop(Map map) : base(map)
        {
            Console.WriteLine("Entering shop... Time to buy some snacks and shiny nonsense! Trade wisely — fortune favors the curious");
            map.gameShop.Open();
        }

        public override void ExitShop()
        {
            map.ChangeState(new StateOnMap(map));
            Console.WriteLine("Thanks for visiting! Come back soon — we already miss your coins!");
        }
    }

    public class MenuState : State
    {
        private State prevState;

        public MenuState(Map map, State prevState) : base(map)
        {
            this.prevState = prevState;

            Console.WriteLine("Choose your option: \n 1. Exit the game \n 2. Return to the game");
            string option = Console.ReadLine();

            if (option == "1")
            {
                Console.WriteLine("Every ending is a new beginning... but in our cats' world" +
                    " there is still hope for a bright future");
                Environment.Exit(0);
            }
            else if (option == "2")
            {
                CloseMenu();
            }
            else
            {
                Console.WriteLine("invalid option");
                CloseMenu();
            }

        }

        public override void CloseMenu()
        {
            Console.WriteLine("Closing menu...");
            map.RestoreState(prevState);
        }
    }


    

    

    
}

