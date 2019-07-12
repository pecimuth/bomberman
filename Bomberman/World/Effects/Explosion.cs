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
    enum ExplosionOrientation
    {
        Vertical = 0,
        Horizontal = 1,
        Central = 2
    }

    class Explosion : Effect
    {
        // koľko Update kým sa zmaže
        private static readonly int ticksLeft = 20;
        // ľavý horný roh v atlas.png prvého typu explózie
        private static readonly Vector2 originVector = new Vector2(32, 288);
        private readonly bool brokeBrick;

        // texture je atlas, location kde sa vykreslí, brokeBrick = bola na mieste location tehla, ktorá sa rozbila?
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

        // výpočet pozície ľavého horného rohu v atlas.png podľa typu
        private static Point GetPointOfOrigin(ExplosionOrientation orientation)
        {
            Vector2 directionVector = new Vector2(Size.X, 0);
            Vector2 resultVector = originVector + (int)orientation * directionVector;
            return resultVector.ToPoint();
        }

        protected override void OnCharactorCollision(Charactor charactor, World world)
        {
            charactor.Damage(world);
        }

        protected override void OnMonsterCollision(Actor monster, World world)
        {
            monster.Damage(world);
        }

        // ak rozbil tehlu, tak sa možno spawne náhodný pickup v jeho mieste
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
