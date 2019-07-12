using Bomberman.World.Actors;
using Bomberman.World.Grids;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman.World.Effects
{
    class Bomb : Effect
    {
        // počet Update kým exploduje
        private static readonly int ticksUntilExplosion = 130;
        private static readonly Point topLeftCornerInTextureAtlas = new Point(0, 288);

        // polomer explózie
        public int ExplosionRadius { get; private set; }

        public Bomb(Texture2D texture, Sector location, int explosionRadius) :
            base(
                 texture
                , true
                , location
                , ticksUntilExplosion
                , topLeftCornerInTextureAtlas
           )
        {
            ExplosionRadius = explosionRadius;
        }

        // vytvorenie explózii, prehranie zvuku, rozbitie tehly
        protected override void OnTimeRanOut(World world)
        {
            world.Audio.Play(Sound.Explosion);
            ++world.Charactor.BombsAvailable;
            world.SpawnExplosion(Location, ExplosionOrientation.Central, false);
            ExplosionsInDirection(0, -1, ExplosionOrientation.Vertical, world);
            ExplosionsInDirection(0, 1, ExplosionOrientation.Vertical, world);
            ExplosionsInDirection(-1, 0, ExplosionOrientation.Horizontal, world);
            ExplosionsInDirection(1, 0, ExplosionOrientation.Horizontal, world);
            Remove();
        }

        // vytvorenie explózii v jednom smere, (deltaX, deltaY) je smerový vektor kolmý na osy
        private void ExplosionsInDirection(int deltaX, int deltaY, ExplosionOrientation orientation, World world)
        {
            for (int i = 1; i <= ExplosionRadius; ++i)
            {
                Sector sector = new Sector(Location.X + i * deltaX, Location.Y + i * deltaY);

                switch (world.Grid.At(sector))
                {
                    case Block.Floor:
                        world.SpawnExplosion(sector, orientation, false);
                        break;

                    case Block.Brick:
                        world.SpawnExplosion(sector, orientation, true);
                        world.Grid.Break(sector);
                        return;

                    default:
                        return;
                }
            }
        }

        protected override void OnCharactorCollision(Charactor charactor, World world)
        { }

        protected override void OnMonsterCollision(Actor monster, World world)
        { }
    }
}
