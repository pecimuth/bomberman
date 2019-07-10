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
        private int bombsAvailable;

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
            }
        }

        public Charactor(Texture2D texture) : base(texture, Appearance.Heroine, Sector.One, 23, new PlayerController(), 3)
        {
            BombRadius = new Stat(1, 1, 1, 6);
            BombsCapacity = new Stat(1, 1, 1, 6);
            bombsAvailable = BombsCapacity.BaseValue;
        }
    }
}
