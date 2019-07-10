using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman
{
    enum ExplosionOrientation
    {
        Vertical = 0,
        Horizontal = 1,
        Central = 2
    }

    class Explosion : Effect
    {
        private static readonly int ticksLeft = 20;
        private static readonly Vector2 originVector = new Vector2(32, 288);
        private readonly bool brokeBrick;

        public Explosion(Texture2D texture, Sector location, ExplosionOrientation orientation, bool brokeBrick) :
            base(
                 texture
                , false
                , location
                , ticksLeft
                , GetPointOfOrigin(orientation)
           )
        {
            this.brokeBrick = brokeBrick;
        }

        private static Point GetPointOfOrigin(ExplosionOrientation orientation)
        {
            Vector2 directionVector = new Vector2(Size.X, 0);
            Vector2 resultVector = originVector + (int)orientation * directionVector;
            return resultVector.ToPoint();
        }

        protected override void OnCharactorCollision(Charactor charactor, World world)
        {
            charactor.Damage();
        }

        protected override void OnMonsterCollision(Actor monster, World world)
        {
            monster.Damage();
        }

        protected override void OnTimeRanOut(World world)
        {
            if (brokeBrick)
            {
                world.MaybeSpawnRandomPickup(Location);
            }
            Remove();
        }
    }
}
