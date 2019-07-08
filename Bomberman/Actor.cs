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
        public WalkingSprite Sprite { get; private set; }
        Controller controller;

        public Actor(Texture2D texture, Appearance appearance, Sector startSector, int ticksPerSector, Controller controller)
        {
            Sprite = new WalkingSprite(texture, appearance, startSector, ticksPerSector);
            this.controller = controller;
        }

        public void Update(KeyboardState keyboardState, World world)
        {
            controller.Update(keyboardState, this, world);
            Sprite.Update();
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            Sprite.Draw(spriteBatch, offset);
        }
    }
}
