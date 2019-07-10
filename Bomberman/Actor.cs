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
        public Stat InvicibilityRemaining { get; private set; }

        public Actor(Texture2D texture, Appearance appearance, Sector startSector, int ticksPerSector, Controller controller, int baseHealth = 1)
        {
            Sprite = new WalkingSprite(texture, appearance, startSector, ticksPerSector);
            Health = new Stat(baseHealth, 1, 0, 3);
            InvicibilityRemaining = new Stat(40, 1, 0, 40);
            this.controller = controller;
        }

        public virtual void Update(KeyboardState keyboardState, World world)
        {
            InvicibilityRemaining.Decrease();
            Sprite.Update();
            controller.Update(keyboardState, this, world);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            bool semiTransparent = InvicibilityRemaining.Value > InvicibilityRemaining.MinValue;
            Sprite.Draw(spriteBatch, offset, semiTransparent);
        }

        public void Damage()
        {
            if (InvicibilityRemaining.Value == InvicibilityRemaining.MinValue)
            {
                Health.Decrease();
                InvicibilityRemaining.Reset();
            }
        }
    }
}
