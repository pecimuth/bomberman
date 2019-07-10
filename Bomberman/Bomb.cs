using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman
{
    class Bomb : Effect
    {
        private static readonly int ticksUntilExplosion = 100;
        private static readonly Point topLeftCornerInTextureAtlas = new Point(0, 288);

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

        protected override void OnTimeRanOut(World world)
        {
            world.SpawnExplosion(Location, ExplosionOrientation.Central, false);
            ExplosionsInDirection(0, -1, ExplosionOrientation.Vertical, world);
            ExplosionsInDirection(0, 1, ExplosionOrientation.Vertical, world);
            ExplosionsInDirection(-1, 0, ExplosionOrientation.Horizontal, world);
            ExplosionsInDirection(1, 0, ExplosionOrientation.Horizontal, world);
        }

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
