using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApp1
{
    // група ворогів 
    public class EnemyGroup
    {
        public string Name { get; }

        // множина ворогів, які входять у групу (композиція)
        public List<Enemy> Enemies { get; }

        public EnemyGroup(string name)
        {
            Name = name;
            Enemies = new List<Enemy>();
        }

        // додати ворога до групи
        public void AddEnemy(Enemy enemy)
        {
            if (enemy == null) return;

            Enemies.Add(enemy);
            Console.WriteLine($"[{Name}] added enemy: {enemy.Name}");
        }

        // вивести стан групи
        public void PrintStatus()
        {
            Console.WriteLine($"\n[{Name}] status:");

            if (Enemies.Count == 0)
            {
                Console.WriteLine("  no enemies in group.");
                return;
            }

            foreach (var e in Enemies)
            {
                Console.WriteLine( $"  - {e.Name}, HP = {e.HP}, alive = {e.IsAlive}");
            }
        }

        // усі живі вороги атакують одного кота
        public void GroupAttack(Cat target, Economy econ)
        {
            Console.WriteLine($"\n[{Name}] attacks {target.Name}:");

            foreach (var e in Enemies)
            {
                if (e.IsAlive)
                {
                    e.AttackTarget(target, econ);
                }
            }
        }

        // усі живі вороги виконують початок ходу
        public void ApplyTurnStart()
        {
            foreach (var e in Enemies)
            {
                if (e.IsAlive)
                {
                    e.OnTurnStart();
                }
            }
        }

        // видалення всіх мертвих ворогів з групи
        public void RemoveDead()
        {
            Enemies.RemoveAll(e => !e.IsAlive);
        }
    
       // пошук першого ворога за ім'ям (або null якщо не знайдено)
        public Enemy FindByName(string name)
        {
            return Enemies.FirstOrDefault(e => e.Name == name);
        }

        // всі вороги певного типу (Garbage, RobotVacuum тощо)
        public List<Enemy> FindByType(EnemyType type)
        {
            return Enemies.Where(e => e.Type == type).ToList();
        }

        // сортування за HP по зростанню
        public void SortByHPAscending()
        {
            Enemies.Sort((a, b) => a.HP.CompareTo(b.HP));
        }

        // сортування за атакою по спаданню (спочатку найсильніші)
        public void SortByAttackDescending()
        {
            Enemies.Sort((a, b) => b.Attack.CompareTo(a.Attack));
        }

   
        public void SaveToFile(string path)
        {
            var lines = new List<string>();

            // перший рядок — ім'я групи (для інформації)
            lines.Add(Name);

            // далі для кожного ворога записуємо лише його тип і ім'я
            foreach (var e in Enemies)
            {
                lines.Add($"{e.Type};{e.Name}");
            }

            File.WriteAllLines(path, lines);
            Console.WriteLine($"[{Name}] saved to file: {path}");
        }

        // очищає поточну колекцію і відновлює склад групи з файлу
        public void LoadFromFile(string path)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine($"File not found: {path}");
                return;
            }

            string[] lines = File.ReadAllLines(path);
            if (lines.Length == 0)
            {
                Console.WriteLine($"File {path} is empty.");
                return;
            }

            Enemies.Clear();

            // пропускаємо перший рядок (там було ім'я групи)
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string[] parts = line.Split(';');
                if (parts.Length < 1)
                    continue;

                if (!Enum.TryParse(parts[0], out EnemyType type))
                    continue;


                // фабричний метод: створює конкретного ворога за його типом
                Enemy enemy = EnemyFactory.CreateEnemy(type);
                if (enemy != null)
                {
                    Enemies.Add(enemy);
                }
            }

            Console.WriteLine($"[{Name}] loaded from file: {path}. Enemies count = {Enemies.Count}");

        }
    }
}
