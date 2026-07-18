using System;

namespace ConsoleApp1
{
    public class State
    {
        protected Map map;
        public State(Map map)
        {
            this.map = map;
        }

        public virtual void SelectedLevel(int levelID)
        {
            Console.WriteLine("This level is locked. Please complete previous levels");
        }

        public virtual void EnterLevel()
        {
            Console.WriteLine("You must select a level to start a fight");
        }

        public virtual void EndLevel()
        {
            Console.WriteLine($"Defeat. Your noble cats have fallen…  Enemies now make them bathe. Rise again, commander.");
        }

        public virtual void EnterShop()
        {
            Console.WriteLine("You can only enter the shop from the map");
        }

        public virtual void ExitShop()
        {
            Console.WriteLine("You are not in the shop right now");
        }

        public virtual void OpenMenu()
        {
            map.ChangeState(new MenuState(map, this));
        }

        public virtual void CloseMenu()
        {
            Console.WriteLine("You are not in the menu right now");
        }

    }
}

