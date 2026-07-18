using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    public abstract class CatFactory
    {
        public abstract Cat CreateCat(string name, int level, int stars);
    }



    public class NinjaFactory : CatFactory
    {
        public override Cat CreateCat(string name, int level, int stars)
        {
            return new Ninja(name, level, stars);
        }
    }

    public class HealerFactory : CatFactory
    {
        public override Cat CreateCat(string name, int level, int stars)
        {
            return new Healer(name, level, stars);
        }
    }

    public class TankFactory : CatFactory
    {
        public override Cat CreateCat(string name, int level, int stars)
        {
            return new Tank(name, level, stars);
        }
    }

    public class BerserkFactory : CatFactory
    {
        public override Cat CreateCat(string name, int level, int stars)
        {
            return new Berserk(name, level, stars);
        }
    }

    public class RandomCatFactory : CatFactory
    {
        private static Random random = new Random();

        private List<CatFactory> factories = new List<CatFactory>
            {
            new NinjaFactory(),
            new HealerFactory(),
            new TankFactory(),
            new BerserkFactory()
            };

        private List<string> names = new List<string>
            {
            "Stepan", "Pinky", "Luna", "Shadow", "Pumpkin", "Misty",
            "Tiger", "Cleo", "Rex", "Bandera", "Pan Shevchenko", "Lesya"
            };

        public override Cat CreateCat(string name, int level, int stars)
        {
            if (name == null)
                name = names[random.Next(names.Count)];

            if (level == 0)
                level = random.Next(1, 6);

            if (stars == 0)
                stars = random.Next(1, 6);

            var factory = factories[random.Next(factories.Count)];
            return factory.CreateCat(name, level, stars);
        }

        public Cat CreateRandomCat()
        {
            return CreateCat(null, 0, 0);
        }

    }
}
