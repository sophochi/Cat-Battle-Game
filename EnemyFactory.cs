using System;

namespace ConsoleApp1
{
    // Factory Method: по значенню enum EnemyType повертає конкретний підклас Enemy

    public static class EnemyFactory
    {
        public static Enemy CreateEnemy(EnemyType type)
        {
            // клієнт знає лише enum-тип, а конкретний клас ворога обирається всередині фабрики
            switch (type)
            {
                case EnemyType.Garbage:
                    return new Garbage();

                case EnemyType.RobotVacuum:
                    return new RobotVacuum();

                case EnemyType.MuscleMouse:
                    return new MuscleMouse();

                case EnemyType.MutantCucumber:
                    return new MutantCucumber();

                case EnemyType.WildGranny:
                    return new WildGranny();

                default:
                    // якщо додали новий тип і не оновили фабрику
                    throw new ArgumentOutOfRangeException(nameof(type),
                        $"Unknown enemy type: {type}");
            }
        }
    }
}
