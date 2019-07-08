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

        protected bool MaybeWalk(Grid grid, WalkingSprite sprite, Facing facing)
        {
            if (IsNeighborFloor(grid, sprite.SectorLocation, facing))
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

        protected bool IsNeighborFloor(Grid grid, Sector sector, Facing facing)
        {
            Sector destination = sector.Neighbor(facing);
            return grid.IsFloor(destination);
        }

        protected bool WalkForwardOrTurnLeft(WalkingSprite sprite, Grid grid)
        {
            int forwardOrientationIndex = Array.IndexOf(orientationsCounterClockwise, sprite.Orientation);
            for (int orientationIter = 0; orientationIter < 4; ++orientationIter)
            {
                int index = (orientationIter + forwardOrientationIndex) % 4;
                Facing facing = orientationsCounterClockwise[index];
                if (MaybeWalk(grid, sprite, facing))
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
    }
}
