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
    class PlayerController : Controller
    {
        // mapovanie tlačítkom klávesnice na orientáciu pohybu
        private static readonly Dictionary<Keys, Facing> keyToOrientation = new Dictionary<Keys, Facing>
        {
            [Keys.Up] = Facing.North,
            [Keys.Down] = Facing.South,
            [Keys.Left] = Facing.West,
            [Keys.Right] = Facing.East
        };

        // minulý Update() bol medzerník stlačený?
        private bool spaceKeyWasDown = false;

        // ovládanie pohybu klávesnicov a kladenie bomby
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

        // skontroluje, či Charactor má dostupnú bombu, ak áno, položí ju pod neho
        private void MaybePlaceBomb(Actor actor, World world)
        {
            if (actor == world.Charactor && world.Charactor.BombsAvailable > 0)
            {
                Sector destination = actor.Sprite.SectorLocationByCentralPoint;
                if (!ContainsMovementRestrictingEffect(destination, world))
                {
                    world.SpawnBomb(destination);
                    --world.Charactor.BombsAvailable;
                    world.Audio.Play(Sound.Drop);
                }
            }
        }
    }
}
