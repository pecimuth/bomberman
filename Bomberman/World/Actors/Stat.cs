using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman.World.Actors
{
    // spravuje nejakú hodnotu, ktorá má základnú hodnotu,
    // minimálnu, maximálnu a dá sa navýšiť/znížiť
    class Stat
    {
        public Stat(int baseValue, int increment, int minValue, int maxValue)
        {
            BaseValue = baseValue;
            Increment = increment;
            MinValue = minValue;
            MaxValue = maxValue;
            Value = baseValue;
        }
        
        // základná hodnota
        public int BaseValue { get; private set; }
        // o koľko sa znízi/zvýši
        public int Increment { get; private set; }
        // minimálna hodnota
        public int MinValue { get; private set; }
        // maximálna hodnota
        public int MaxValue { get; private set; }
        // súčasná hodnota
        public int Value { get; private set; }

        public void Increase()
        {
            Value += Increment;
            Value = Math.Min(MaxValue, Value);
        }

        public void Decrease()
        {
            Value -= Increment;
            Value = Math.Max(MinValue, Value);
        }

        // reset na základnú hodnotu
        public void Reset()
        {
            Value = BaseValue;
        }
    }
}
