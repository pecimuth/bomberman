using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman
{
    class PlayerController
    {
        private static readonly Dictionary<Keys, Facing> keyToOrientation = new Dictionary<Keys, Facing>
        {
            [Keys.Up] = Facing.North,
            [Keys.Down] = Facing.South,
            [Keys.Left] = Facing.West,
            [Keys.Right] = Facing.East
        };
   
        public void Update(KeyboardState keyboardState, WalkingSprite sprite)
        {
            foreach (var pair in keyToOrientation)
            {
                if (keyboardState.IsKeyDown(pair.Key))
                {
                    sprite.Walk(pair.Value);
                }
            }
        }
    }
}
