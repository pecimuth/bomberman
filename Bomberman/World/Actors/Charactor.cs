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
        // polomer dosahu explózie bomby v počte sektorov 
        public Stat BombRadius { get; private set; }
        // koľko bomb može naraz položiť
        public Stat BombsCapacity { get; private set; }
        // koľko bomb má práve dostupných na položenie
        private int bombsAvailable;

        // dostupných bomb je maximálne toľko ako kapacita
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

        // startSector - kde sa objaví nazačiatku
        public Charactor(Texture2D texture, Sector startSector) : base(texture, Appearance.Heroine, startSector, 23, new PlayerController(), 3)
        {
            BombRadius = new Stat(1, 1, 1, 6);
            BombsCapacity = new Stat(1, 1, 1, 6);
            bombsAvailable = BombsCapacity.BaseValue;
        }

        // zranenie Charactove - prehranie zvukového efektu a update stavu levelu navyše
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
