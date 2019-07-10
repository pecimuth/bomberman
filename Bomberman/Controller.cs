using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman
{
    abstract class Controller
    {
        protected static readonly Facing[] orientationsCounterClockwise = new Facing[] { Facing.East, Facing.North, Facing.West, Facing.South };

        public abstract void Update(KeyboardState keyboardState, Actor actor, World world);

        protected bool MaybeWalk(World world, WalkingSprite sprite, Facing facing)
        {
            if (IsNeighborFloor(world, sprite.SectorLocation, facing))
            {
                sprite.Walk(facing);
                return true;
            }
            else if (!sprite.Moving)
            {
                sprite.Orientation = facing;
            }

            return false;
        }

        protected bool IsNeighborFloor(World world, Sector sector, Facing facing)
        {
            Sector destination = sector.Neighbor(facing);

            return world.Grid.IsFloor(destination) && !ContainsMovementRestrictingEffect(destination, world);
        }

        protected bool ContainsMovementRestrictingEffect(Sector sector, World world)
        {
            return world
                .Effects
                .FindAll((Effect effect) => effect.Location == sector)
                .Exists((Effect effect) => effect.RestrictActorMovement);
        }

        protected bool WalkForwardOrTurnLeft(WalkingSprite sprite, World world)
        {
            int forwardOrientationIndex = Array.IndexOf(orientationsCounterClockwise, sprite.Orientation);
            for (int orientationIter = 0; orientationIter < 4; ++orientationIter)
            {
                int index = (orientationIter + forwardOrientationIndex) % 4;
                Facing facing = orientationsCounterClockwise[index];
                if (MaybeWalk(world, sprite, facing))
                {
                    return orientationIter == 0;
                }
            }
            return false;
        }

        protected Facing RotateRight(Facing facing)
        {
            int orientationIndex = Array.IndexOf(orientationsCounterClockwise, facing);
            --orientationIndex;
            if (orientationIndex < 0)
            {
                orientationIndex += 4;
            }

            return orientationsCounterClockwise[orientationIndex];
        }

        protected Facing Rotate180(Facing facing)
        {
            return RotateRight(RotateRight(facing));
        }

        protected void TurnBackIfWalkingTowardsObstacle(WalkingSprite sprite, World world)
        {
            if (sprite.Moving && ContainsMovementRestrictingEffect(sprite.DestinationSector, world))
            {
                MaybeWalk(world, sprite, Rotate180(sprite.Orientation));
            }
        }
    }
}
