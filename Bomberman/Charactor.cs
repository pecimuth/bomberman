using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman
{
    class Charactor : Actor
    {
        public Stat BombRadius { get; private set; }
        public Stat BombsCapacity { get; private set; }
        public Stat BombReloadTime { get; private set; }
        private int bombsAvailable;
        private int bombReloadCounter = 0;
        public int BombsAvailable
        {
            get
            {
                return bombsAvailable;
            }
            set
            {
                bombsAvailable = value;
                bombsAvailable = Math.Max(bombsAvailable, 0);
                bombsAvailable = Math.Min(BombsCapacity.Value, bombsAvailable);
                bombReloadCounter = 0;
            }
        }

        public Charactor(Texture2D texture) : base(texture, Appearance.Heroine, Sector.One, 23, new PlayerController())
        {
            BombRadius = new Stat(1, 1, 1, 6);
            BombsCapacity = new Stat(1, 1, 1, 6);
            BombReloadTime = new Stat(100, 10, 50, 100);
            bombsAvailable = BombsCapacity.BaseValue;
        }

        public override void Update(KeyboardState keyboardState, World world)
        {
            if (BombsAvailable < BombsCapacity.Value)
            {
                ++bombReloadCounter;
                if (bombReloadCounter >= BombReloadTime.Value)
                {
                    ++bombsAvailable;
                }
            }

            base.Update(keyboardState, world);
        }
    }
}
