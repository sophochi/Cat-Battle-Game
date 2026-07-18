using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    public enum LevelStatus
    {
        Locked,
        Unlocked,
        Completed
    }


    public class Level
    {
        public int ID { get; private set; }
        public string Name { get; private set; }
        public EnemyType enemyType { get; private set; }
        public int EnemyCount { get; private set; }


        public LevelStatus Status { get; set; }

        public Level(int id, string name, EnemyType enemyType, int enemy_count)
        {
            ID = id;
            Name = name;
            this.enemyType = enemyType;
            EnemyCount = enemy_count;
            Status = LevelStatus.Locked;
        }

        public void RegisterEnemyDefeated() // реєстрація смерті одного ворога
        {
            if (EnemyCount > 0)
            {
                EnemyCount--;
            }
        }

    }

    public static class Levels
    {
        public static List<Level> All { get; private set; }

        static Levels()
        {
            All = new List<Level>
            {
                new Level(1, "Swamp Rotters", EnemyType.Garbage, 4),
                new Level(2, "Vacuum Arena", EnemyType.RobotVacuum, 5),
                new Level(3, "Ratropolis", EnemyType.MuscleMouse, 8),
                new Level(4, "Cucumber Jungle", EnemyType.MutantCucumber, 2),
                new Level(5, "Cursed Clinic", EnemyType.WildGranny, 1)
            };

            All[0].Status = LevelStatus.Unlocked;
        }

        public static void Unlock(Level level)
        {
            if (level.Status == LevelStatus.Locked) level.Status = LevelStatus.Unlocked;
        }

        public static void CompletedLevel(Level level)
        {
            if (level == null || level.Status != LevelStatus.Unlocked) return;

            level.Status = LevelStatus.Completed;

            Level next_level = FindByID(level.ID + 1);
            if (next_level != null)
                next_level.Status = LevelStatus.Unlocked;
        }

        public static bool IsUnlocked(Level level)
        {
            if (level == null) return false;
            return level.Status == LevelStatus.Unlocked;
        }

        public static Level FindByID(int id)
        {
            if (id <= 0) return null;
            foreach (Level level in All)
            {
                if (level.ID == id) return level;
            }
            return null;
        }

        public static bool IsVictory(Level level)
        {
            // перемога, якщо ворогів не лишилося
            return level != null && level.EnemyCount == 0;
        }

    }
}
