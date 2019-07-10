using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman
{
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

        public int BaseValue { get; private set; }
        public int Increment { get; private set; }
        public int MinValue { get; private set; }
        public int MaxValue { get; private set; }
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
    }
}
