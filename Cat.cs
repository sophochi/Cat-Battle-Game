using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    //Основний клас котиків
    public abstract class Cat : Entity
    {
        public Paw LeftPaw { get; protected set; }
        public int Level { get; protected set; }
        public Inventory Inventory { get; protected set; }
        public int QuantityOfStars { get; protected set; }

        public Cat(string name, int level, int stars)
            : base(name, maxHp: 1, attack: 1, defense: 1)
        {
            Level = level;
            Inventory = new Inventory(3 * Level);
            LeftPaw = new Paw();
            QuantityOfStars = stars;

            SetBaseStats(); 
        }

        public void ShowCardInfo()
        {
            Console.WriteLine($"[{GetType().Name}] {Name} (Lv. {Level}) - HP: {HP}, ATK: {Attack}, DEF: {Defense}, Stars: {QuantityOfStars}");
        }

        public bool CanBeMergedWith(Cat other)
        {
            return GetType() == other.GetType() && Level == other.Level;
        }

        public Cat MergeWith(Cat other)
        {
            if (!CanBeMergedWith(other))
            {
                Console.WriteLine("It is imposible to merge this cards");
                return null;
            }

            int MergedStars = 0;
            if (QuantityOfStars + other.QuantityOfStars > 5)
            {
                MergedStars = 5;
            }
            else
            {
                MergedStars = QuantityOfStars + other.QuantityOfStars;
            }
            Console.WriteLine($"Merging of this two {GetType()} gave {MergedStars} stars");
            Cat upgradedCat = Activator.CreateInstance(GetType(), Name, Level) as Cat;
            upgradedCat.QuantityOfStars = MergedStars;

            return upgradedCat;
        }

        public virtual void AttackTarget(Enemy target)
        {
            if (!IsAlive)
            {
                Console.WriteLine($"{Name} can't attack, bc he is dead");
                return;
            }

            // ефект Stunned виставляється в Entity.OnTurnStart()
            if (IsStunnedThisTurn)
            {
                Console.WriteLine($"{Name} is stunned and can't attack this turn");
                return;
            }

            Console.WriteLine($"{Name} attacks {target.Name}!");
            int damage = (Attack * QuantityOfStars) - target.Defense;
            if (damage <= 0) damage = 0;

            target.TakeDamage(damage);
            Console.WriteLine($"Damage is {damage}");
        }

        protected abstract void SetBaseStats();

        public void TakeItemInPaw(Item item)
        {
            if (!LeftPaw.IsFree)
            {
                Console.WriteLine("Paw is taken");
                return;
            }
            LeftPaw.TakeItem(item);
        }

        public void DropItemFromPaw()
        {
            LeftPaw.DropItem();
        }

        public void OpenInventory()
        {
            Inventory.OpenBackpack();
        }

        public void AddItemInInventory(Item item)
        {
            Inventory.AddItem(item);
        }

        public virtual void Heal(Cat target)
        {
            Console.WriteLine($"{Name} cannot heal");
        }

        public void RestoreHP(int amount)
        {
            if(!IsAlive)
            {
                Console.WriteLine("The cat is dead and cannot be healed");
                return;
            }

            HP = Math.Min(MaxHP, HP + amount);
        }

        public virtual void StartTurn()
        {
            // тут відпрацює отрута, stun, slow для кота
            OnTurnStart();

            Console.WriteLine(
                $"{Name} turn start: HP={HP}, Effects={ActiveEffects}, Stunned={IsStunnedThisTurn}");
        }

    }

    //Підкласи котиків
    public class Ninja : Cat
    {
        public Ninja(string name, int level, int stars) : base(name, level, stars) { }

        protected override void SetBaseStats()
        {
            Attack = 3 + Level * 3;
            Defense = 2 + Level;
            MaxHP = 2 + Level * 3;
            HP = MaxHP;
        }

    }

    public class Healer : Cat
    {
        public Healer(string name, int level, int stars) : base(name, level, stars) { }

        protected override void SetBaseStats()
        {
            Attack = 3 + Level * 2;
            Defense = 3 + Level * 2;
            MaxHP = 5 + Level * 3;
            HP = MaxHP;
        }
        
        public override void Heal(Cat target)
        {
            target.RestoreHP(Attack);
            Console.WriteLine($"{Name} healed {target.Name} for {Attack} HP. HP is now {target.HP}");
        }
    }

    public class Tank : Cat
    {
        public Tank(string name, int level, int stars) : base(name, level, stars) { }

        protected override void SetBaseStats()
        {
            MaxHP = 10 + Level * 3;
            HP = MaxHP;
            Attack = 3 + Level * 2;
            Defense = 3 + Level * 3;
        }

        public bool ProtectTarget(Cat target, int damage)
        {
            if (!target.IsAlive)
            {
                Console.WriteLine($"{target.Name} is dead and cannot be protected.");
                return false;
            }

            if (damage > Defense)
            {
                int causedDamage = damage - Defense;
                TakeDamage(causedDamage);
                Console.WriteLine($"{Name} lost {causedDamage} damage.");
                return true;
            } else
            {
                Console.WriteLine($"{Name} protected {target.Name} without losing HP");
                return true;
            }
        }
    }

    public class Berserk : Cat
    {
        private bool rageActivated = false;
        public Berserk(string name, int level, int stars) : base(name, level, stars) { }

        protected override void SetBaseStats()
        {
            MaxHP = 5 + Level * 3;
            HP = MaxHP;
            Attack = 3 + Level * 3;   
            Defense = 2 + Level;
        }
        
        public void RageMode()
        {
            if (rageActivated) return;

            rageActivated = true;
            Attack *= 2;
            Defense -= 2;

            Console.WriteLine($"{Name} is now in a rage mode! Attack {Attack}, Defence {Defense}");
        }

        public override void TakeDamage(int attack)
        {
            base.TakeDamage(attack);

            if(HP < 3 && !rageActivated)
            {
                RageMode();
            }
        }
    }
}
