using Bomberman.World.Actors.Controllers;
using Bomberman.World.Actors.Sprite;
using Bomberman.World.Grids;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman.World.Actors
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

        public Charactor(Texture2D texture, Sector startSector) : base(texture, Appearance.Heroine, startSector, 23, new PlayerController(), 3)
        {
            BombRadius = new Stat(1, 1, 1, 6);
            BombsCapacity = new Stat(1, 1, 1, 6);
            bombsAvailable = BombsCapacity.BaseValue;
        }

        public override void Damage(World world)
        {
            base.Damage(world);
            world.Audio.Play(Sound.Hurt);

            if (Health.Value == Health.MinValue && world.LevelState == LevelState.InProgress)
            {
                world.LevelState = LevelState.Failed;
            }
        }
    }
}
