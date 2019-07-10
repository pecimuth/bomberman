using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman
{
    class PlayerController : Controller
    {
        private static readonly Dictionary<Keys, Facing> keyToOrientation = new Dictionary<Keys, Facing>
        {
            [Keys.Up] = Facing.North,
            [Keys.Down] = Facing.South,
            [Keys.Left] = Facing.West,
            [Keys.Right] = Facing.East
        };

        private bool spaceKeyWasDown = false;
 
        public override void Update(KeyboardState keyboardState, Actor actor, World world)
        {
            Grid grid = world.Grid;
            WalkingSprite sprite = actor.Sprite;
            foreach (var pair in keyToOrientation)
            {
                if (keyboardState.IsKeyDown(pair.Key))
                {
                    MaybeWalk(world, actor.Sprite, pair.Value);
                }
            }

            if (keyboardState.IsKeyDown(Keys.Space))
            {
                if (!spaceKeyWasDown)
                {
                    MaybePlaceBomb(actor, world);
                }

                spaceKeyWasDown = true;
            }
            else
            {
                spaceKeyWasDown = false;
            }
        }

        private void MaybePlaceBomb(Actor actor, World world)
        {
            if (actor == world.Charactor && world.Charactor.BombsAvailable > 0)
            {
                --world.Charactor.BombsAvailable;
                Sector destination = actor.Sprite.SectorLocationByCentralPoint;
                if (!ContainsMovementRestrictingEffect(destination, world))
                {
                    world.SpawnBomb(destination);
                }
            }
        }
    }
}
