using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bomberman.World.Grids;
using Microsoft.Xna.Framework.Input;

namespace Bomberman.World.Actors.Controllers
{
    class ChaseCharactorController : Controller
    {
        // keď vidí Charactor, ide za ním
        // inak chodí rovno a keď to nie je možné, zatáča vľavo
        public override void Update(KeyboardState keyboardState, Actor actor, World world)
        {
            if (actor.Sprite.Moving)
            {
                TurnBackIfWalkingTowardsObstacle(actor.Sprite, world);
                return;
            }

            Sector charactorLocation = world.Charactor.Sprite.SectorLocation;
            Sector myLocation = actor.Sprite.SectorLocation;

            if (myLocation != charactorLocation)
            {
                if (IsInLineOfSight(myLocation, charactorLocation, world.Grid, out Facing nextStep))
                {
                    MaybeWalk(world, actor.Sprite, nextStep);
                }
                else
                {
                    WalkForwardOrTurnLeft(actor.Sprite, world);
                }
            }
        }

        // je sektor end viditeľný zo sektoru start?
        // viditeľný znamená že je medzi nimi iba podľaha a nejakú súradnicu majú rovnaku
        // vráti orientáciu smerom zo start do end
        private bool IsInLineOfSight(Sector start, Sector end, Grid grid, out Facing orientation)
        {
            orientation = Facing.West;

            if (start.X == end.X)
            {
                for (int y = Math.Min(start.Y, end.Y); y <= Math.Max(start.Y, end.Y); ++y)
                {
                    if (!grid.IsFloor(new Sector(start.X, y)))
                    {
                        return false;
                    }
                }

                orientation = end.Y > start.Y ? Facing.South : Facing.North;
                return true;
            }
            else if (start.Y == end.Y)
            {
                for (int x = Math.Min(start.X, end.X); x <= Math.Max(start.X, end.X); ++x)
                {
                    if (!grid.IsFloor(new Sector(x, start.Y)))
                    {
                        return false;
                    }
                }

                orientation = end.X > start.X ? Facing.East : Facing.West;
                return true;
            }
            return false;
        }
    }
}
