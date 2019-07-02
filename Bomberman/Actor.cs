using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman
{
    class Actor
    {
        WalkingSprite sprite;
        PlayerController controller;

        public Actor(Texture2D texture)
        {
            sprite = new WalkingSprite(texture, Appearance.Heroine, Sector.Zero);
            controller = new PlayerController();
        }

        public void Update(KeyboardState keyboardState)
        {
            controller.Update(keyboardState, sprite);
            sprite.Update();
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            sprite.Draw(spriteBatch, offset);
        }
    }
}
