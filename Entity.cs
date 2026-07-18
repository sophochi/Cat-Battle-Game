using System;

namespace ConsoleApp1
{
    public abstract class Entity
    {
        public string Name { get; protected set; }
        public int HP { get; protected set; }
        public int MaxHP { get; protected set; }
        public int Attack { get; protected set; }
        public int Defense { get; protected set; }

        public bool IsAlive { get; protected set; } = true;

        // ефекти стану 
        public Effects ActiveEffects { get; private set; } = Effects.None;

        // тривалість кожного ефекту в ходах
        private int _poisonTurns;
        private int _stunTurns;
        private int _slowTurns;

        // прапорець що показує чи істота оглушена на поточний хід
        public bool IsStunnedThisTurn { get; private set; }

        protected Entity(string name, int maxHp, int attack, int defense)
        {
            Name = name;
            MaxHP = maxHp;
            HP = maxHp;
            Attack = attack;
            Defense = defense;
        }

        // базове нанесення шкоди 
        public virtual void TakeDamage(int attackValue)
        {
            // сирий удар мінус броня
            int damage = attackValue - Defense;

            // якщо броня повністю поглинула удар – нічого не робимо
            if (damage <= 0)
            {
                return;
            }

            HP -= damage;

            if (HP <= 0)
            {
                HP = 0;
                IsAlive = false;
            }

            Console.WriteLine($"{Name} got {damage} damage and now HP is {HP}");
        }

        // універсальний метод атаки 
        public virtual void AttackTarget(Entity target)
        {
            if (!IsAlive)
            {
                Console.WriteLine($"{Name} can't attack, because it is dead");
                return;
            }

            Console.WriteLine($"{Name} attacks {target.Name}!");
            target.TakeDamage(Attack);
            Console.WriteLine($"Attack is {Attack}");
        }

        // накладання ефекту на істоту 
        public virtual void ApplyEffect(Effects effect, int durationTurns = 1)
        {
            if (effect == Effects.None) return;
            if (durationTurns <= 0) return;

            ActiveEffects |= effect;

            if (effect.HasFlag(Effects.Poisoned))
            {
                if (_poisonTurns < durationTurns) _poisonTurns = durationTurns;
            }

            if (effect.HasFlag(Effects.Stunned))
            {
                if (_stunTurns < durationTurns) _stunTurns = durationTurns;
            }

            if (effect.HasFlag(Effects.Slowed))
            {
                if (_slowTurns < durationTurns) _slowTurns = durationTurns;
            }
        }

        // обробка ефектів на початку ходу 
        public virtual void OnTurnStart()
        {
            // періодична шкода від отрути
            if (_poisonTurns > 0 && IsAlive)
            {
                int poisonDmg = 1;
                Console.WriteLine($"{Name} suffers poison tick ({poisonDmg} dmg).");

                
                TakeDamage(poisonDmg);
            }

            // поточний стан оглушений на цей хід
            IsStunnedThisTurn = _stunTurns > 0;

            // зменшуємо лічильники та скидаємо прапорці по завершенню ефектів

            if (_poisonTurns > 0)
            {
                _poisonTurns--;
                if (_poisonTurns == 0)
                {
                    ActiveEffects &= ~Effects.Poisoned;
                }
            }

            if (_stunTurns > 0)
            {
                _stunTurns--;
                if (_stunTurns == 0)
                {
                    ActiveEffects &= ~Effects.Stunned;
                }
            }

            if (_slowTurns > 0)
            {
                _slowTurns--;
                if (_slowTurns == 0)
                {
                    ActiveEffects &= ~Effects.Slowed;
                }
            }
        }
    }
}