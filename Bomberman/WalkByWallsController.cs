using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace Bomberman
{
    class WalkByWallsController : Controller
    {
        public override void Update(KeyboardState keyboardState, Actor actor, World world)
        {
            Facing right = RotateRight(actor.Sprite.Orientation);
            Sector rightSector = actor.Sprite.SectorLocation.Neighbor(right);

            if (world.Grid.IsFloor(rightSector))
            {
                MaybeWalk(world.Grid, actor.Sprite, right);
            }
            else
            {
                WalkForwardOrTurnLeft(actor.Sprite, world.Grid);
            }
        }
    }
}
