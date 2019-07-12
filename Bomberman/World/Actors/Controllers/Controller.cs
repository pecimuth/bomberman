using Bomberman.World.Actors.Sprite;
using Bomberman.World.Grids;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman.World.Actors.Controllers
{
    abstract class Controller
    {
        // orientácie v poradí proti smeru hodinových ručičiek
        protected static readonly Facing[] orientationsCounterClockwise = new Facing[] { Facing.East, Facing.North, Facing.West, Facing.South };

        public abstract void Update(KeyboardState keyboardState, Actor actor, World world);

        // keď je na cieľovom políčku podlaha, tak pohni sprite a vráť true, inak false
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

        // vypoćíta susedné políčko podľa orientácie, vráti či je to podlaha
        protected bool IsNeighborFloor(World world, Sector sector, Facing facing)
        {
            Sector destination = sector.Neighbor(facing);

            return world.Grid.IsFloor(destination) && !ContainsMovementRestrictingEffect(destination, world);
        }

        // je na sektore nejaký efekt, ktorý má RestrictActorMovement rovný true?
        protected bool ContainsMovementRestrictingEffect(Sector sector, World world)
        {
            return world
                .Effects
                .FindAll((effect) => effect.Location == sector)
                .Exists((effect) => effect.RestrictActorMovement);
        }

        // skús ísť rovno keď sa dá, inak vľavo a opakuj
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

        // vráti novú orientáciu zrotovanú o 90 stupňov pravo
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

        // vráti orientáciu zrotovanú o 180 stupňov
        protected Facing Rotate180(Facing facing)
        {
            return RotateRight(RotateRight(facing));
        }

        // sprite sa otočí o 180 stupňov keď kráča naproti efektu s ActorRestrictMovement == true
        protected void TurnBackIfWalkingTowardsObstacle(WalkingSprite sprite, World world)
        {
            if (sprite.Moving && ContainsMovementRestrictingEffect(sprite.DestinationSector, world))
            {
                MaybeWalk(world, sprite, Rotate180(sprite.Orientation));
            }
        }
    }
}
