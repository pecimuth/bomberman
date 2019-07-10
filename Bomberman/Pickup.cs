using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman
{
    enum PickupType
    {
        BombsCapacity = 0,
        Health = 1,
        BombRadius = 2,
        MovementSpeed = 3
    }

    class Pickup : Effect
    {
        private static readonly Vector2 originVector = new Vector2(0, 320);
        private readonly PickupType pickupType;

        public Pickup(Texture2D texture, Sector location, PickupType pickupType) :
            base(
                texture
                , false
                , location
                , 0
                , GetPointOfOrigin(pickupType)
            )
        {
            this.pickupType = pickupType;
        }

        private static Point GetPointOfOrigin(PickupType pickupType)
        {
            Vector2 directionVector = new Vector2(Size.X, 0);
            Vector2 resultVector = originVector + (int)pickupType * directionVector;
            return resultVector.ToPoint();
        }
 
        protected override void OnCharactorCollision(Charactor charactor, World world)
        {
            switch (pickupType)
            {
                case PickupType.BombsCapacity:
                    charactor.BombsCapacity.Increase();
                    ++charactor.BombsAvailable;
                    break;
                case PickupType.BombRadius:
                    charactor.BombRadius.Increase();
                    break;
                case PickupType.Health:
                    charactor.Health.Increase();
                    break;
                case PickupType.MovementSpeed:
                    charactor.Sprite.MovementSpeed.Decrease();
                    break;
            }
            Remove();
        }

        protected override void OnMonsterCollision(Actor monster, World world)
        { }

        protected override void OnTimeRanOut(World world)
        { }
    }
}
