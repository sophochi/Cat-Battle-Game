using System;

namespace ConsoleApp1
{
    #region Lists and auxiliary things

    public enum EnemyType
    { Garbage, RobotVacuum, MuscleMouse, MutantCucumber, WildGranny }

    [Flags]
    public enum Effects
    {
        None = 0,
        Poisoned = 1 << 0, // periodic damage
        Stunned = 1 << 1,  // skip next action
        Slowed = 1 << 2    // less steps per turn
    }

    #endregion

    #region Enemy base

    public abstract class Enemy : Entity
    {
        protected static readonly Random Rng = new Random();

        public EnemyType Type { get; protected set; } // тип ворога (enum)
        public int Range { get; protected set; } // дальність атаки
        public int Speed { get; protected set; }  // швидкість ворога

        // ефективна швидкість (з урахуванням ефекту Slowed)
        public int EffectiveSpeed
        {
            get
            {
                if (ActiveEffects.HasFlag(Effects.Slowed))
                {
                    int slowed = Speed - 1;
                    if (slowed < 0) slowed = 0;
                    return slowed;
                }
                return Speed;
            }
        }

        protected Enemy(
            EnemyType type,
            string name,
            int hp,
            int atk,
            int def,
            int range,
            int speed)
            : base(name, maxHp: hp, attack: atk, defense: def)
        {
            Type = type;
            Range = range;
            Speed = speed;

            MaxHP = hp;
        }

        // поліморфна атака ворога по коту
        public virtual void AttackTarget(Cat target, Economy econ = null)
        {
            if (!IsAlive) return;

            if (IsStunnedThisTurn)
            {
                Console.WriteLine($"{Name} is stunned and skips the action.");
                return;
            }

            Console.WriteLine($"{Name} attacks {target.Name} for {Attack} (raw).");
            target.TakeDamage(Attack);
        }

        public override void TakeDamage(int dmg)
        {
            base.TakeDamage(dmg);

            if (!IsAlive)
            {
                Console.WriteLine($"{Name} defeated!!!");
            }
        }
    }

    #endregion

    #region Specific enemies (specifications)

    // 1) Garbage — слабкий, часто промахується
    public sealed class Garbage : Enemy
    {
        public Garbage()
            : base(EnemyType.Garbage, "Garbage", hp: 8, atk: 2, def: 0, range: 1, speed: 1)
        {
        }

        public override void AttackTarget(Cat target, Economy econ = null)
        {
            if (Rng.NextDouble() < 0.30)
            {
                Console.WriteLine($"{Name} missed {target.Name}!");
                return;
            }
            Console.WriteLine($"{Name} hits on {target.Name} (attack {Attack}).");
            target.TakeDamage(Attack);
        }
    }

    // 2) RobotVacuum — може вкрасти кристал
    public sealed class RobotVacuum : Enemy
    {
        public RobotVacuum()
            : base(EnemyType.RobotVacuum, "Robot Vacuum", 12, 3, 2, 1, 2)
        {
        }

        public override void AttackTarget(Cat target, Economy econ = null)
        {
            Console.WriteLine($"{Name} vacuum {target.Name} (attack {Attack}).");
            target.TakeDamage(Attack);

            if (econ != null && Rng.NextDouble() < 0.20)
            {
                Console.WriteLine($"{Name} trying to steal the crystal");
                econ.TrySteal(1);
            }
        }
    }

    // 3) MuscleMouse — божеволіє при HP <= 50%, разово піднімає атаку
    public sealed class MuscleMouse : Enemy
    {
        private bool getCrazy = false;

        public MuscleMouse()
            : base(EnemyType.MuscleMouse, "Muscle Mouse", 16, 5, 3, 1, 2)
        {
        }

        public override void OnTurnStart()
        {
            // спочатку застосовуємо загальну обробку ефектів (отрута, stun, slow)
            base.OnTurnStart();

            // додаткова унікальна поведінка MuscleMouse
            if (!getCrazy && HP <= MaxHP / 2)
            {
                getCrazy = true;
                Attack += 2;
                Console.WriteLine($"{Name} get CRAZY! Attack now {Attack}.");
            }
        }

        public override void AttackTarget(Cat target, Economy econ = null)
        {
            Console.WriteLine($"{Name} hits {target.Name} (attack {Attack}).");
            target.TakeDamage(Attack);
        }
    }

    // 4) MutantCucumber — плевок з шансом оглушити
    public sealed class MutantCucumber : Enemy
    {
        public MutantCucumber()
            : base(EnemyType.MutantCucumber, "Mutant Cucumber", 20, 6, 4, 2, 1)
        {
        }

        public override void AttackTarget(Cat target, Economy econ = null)
        {
            Console.WriteLine($"{Name} spits poison in {target.Name} (attack {Attack}, range {Range}).");
            target.TakeDamage(Attack);

            if (Rng.NextDouble() < 0.30)
            {
                Console.WriteLine($"{target.Name} stunned and missed the next action");
                target.ApplyEffect(Effects.Stunned, 1);
            }
        }
    }

    // 5) WildGranny — або перегодовує (slow), або кидає спиці (сильніша атака)
    public sealed class WildGranny : Enemy
    {
        public WildGranny()
            : base(EnemyType.WildGranny, "Wild Granny", 25, 4, 5, 1, 1)
        {
        }

        public override void AttackTarget(Cat target, Economy econ = null)
        {
            bool overfeed = Rng.NextDouble() < 0.40;

            if (overfeed)
            {
                Console.WriteLine($"{Name} overfeeding {target.Name}: gets slower ");
                // уповільнення на кота
                target.ApplyEffect(Effects.Slowed, 2);
            }
            else
            {
                int spikeAttack = Attack + 2;
                Console.WriteLine($"{Name} throws knitting needles at {target.Name}! Long-range attack {spikeAttack} (partially ignores defense)");
                target.TakeDamage(spikeAttack);
            }
        }
    }

    #endregion
}

