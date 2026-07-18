using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    public interface IEconomyObserver
    {
        void Update(int newCrystalCount);
    }

    public interface IEconomyObservable
    {
        void Subscribe(IEconomyObserver o);
        void Unsubscribe(IEconomyObserver o);
        void Notify();
    }
        
    public class Economy : IEconomyObservable
    {
        private List<IEconomyObserver> _observers = new List<IEconomyObserver>();

        private int _crystals;
        public int Crystals
        {
            get { return _crystals; }
            set
            {
                if (_crystals != value)
                {
                    _crystals = value;
                    Notify();
                }
            }
        }

        public Economy(int crystals)
        {
            _crystals = crystals;
        }

        public bool TrySteal(int amount)
        {
            if (Crystals <= 0) return false;

            int stolen = Math.Min(amount, Crystals);
            Crystals -= stolen;
            Console.WriteLine($"[Economy] {stolen} crystal(s) stolen! {Crystals} left.");
            return true;
        }

        public void Subscribe(IEconomyObserver observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
        }

        public void Unsubscribe(IEconomyObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify()
        {
            foreach (var observer in _observers.ToList())
            {
                observer.Update(this.Crystals);
            }
        }
    }
}
