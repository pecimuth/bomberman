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
        public Stat Health { get; private set; }

        public Actor(Texture2D texture, Appearance appearance, Sector startSector, int ticksPerSector, Controller controller)
        {
            Sprite = new WalkingSprite(texture, appearance, startSector, ticksPerSector);
            Health = new Stat(1, 1, 0, 3);
            this.controller = controller;
        }

        public virtual void Update(KeyboardState keyboardState, World world)
        {
            Sprite.Update();
            controller.Update(keyboardState, this, world);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            Sprite.Draw(spriteBatch, offset);
        }

        public void Damage()
        {
            Health.Decrease();
        }
    }
}
