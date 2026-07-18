using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    public class Map
    {
        public List<Level> levels { get; private set; }
        public int currLevelID { get; set; }
        public Economy gameEconomy { get; private set; }
        private State state;
        public Shop gameShop { get; private set; }

        public Map(Economy economy, Shop shop)
        {
            this.state = new StateOnMap(this);
            this.levels = Levels.All;
            this.gameEconomy = economy;
            this.gameShop = shop;
            Console.WriteLine("You are now on the map:)");
        }

        public void ChangeState(State newState)
        {
            state = newState;
        }

        public void RestoreState(State prevState)
        {
            state = prevState;
            Console.WriteLine($"(gamestate restored back to {state})");
        }

        public void SelectedLevel(int levelID)
        {
            state.SelectedLevel(levelID);
        }

        public void EnterLevel()
        {
            state.EnterLevel();
        }

        public void EndLevel()
        {
            state.EndLevel();
        }

        public void EnterShop()
        {
            state.EnterShop();
        }

        public void ExitShop()
        {
            state.ExitShop();
        }

        public void OpenMenu()
        {
            state.OpenMenu();
        }

        public void CloseMenu()
        {
            state.CloseMenu();
        }

    }
}

