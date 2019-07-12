using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bomberman.World.Grids;
using Microsoft.Xna.Framework.Input;

namespace Bomberman.World.Actors.Controllers
{
    class WalkByWallsController : Controller
    {
        // chodí vedľa stien
        public override void Update(KeyboardState keyboardState, Actor actor, World world)
        {
            if (actor.Sprite.Moving)
            {
                TurnBackIfWalkingTowardsObstacle(actor.Sprite, world);
                return;
            }

            Facing right = RotateRight(actor.Sprite.Orientation);
            Sector rightSector = actor.Sprite.SectorLocation.Neighbor(right);

            if (world.Grid.IsFloor(rightSector))
            {
                MaybeWalk(world, actor.Sprite, right);
            }
            else
            {
                WalkForwardOrTurnLeft(actor.Sprite, world);
            }
        }
    }
}
