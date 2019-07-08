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
   
        public override void Update(KeyboardState keyboardState, Actor actor, World world)
        {
            Grid grid = world.Grid;
            WalkingSprite sprite = actor.Sprite;
            foreach (var pair in keyToOrientation)
            {
                if (keyboardState.IsKeyDown(pair.Key))
                {
                    MaybeWalk(world.Grid, actor.Sprite, pair.Value);
                }
            }
        }
    }
}
