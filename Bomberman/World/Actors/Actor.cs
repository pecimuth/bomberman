using Bomberman.World.Actors.Controllers;
using Bomberman.World.Actors.Sprite;
using Bomberman.World.Grids;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman.World.Actors
{
    class Actor
    {
        public WalkingSprite Sprite { get; private set; }
        Controller controller;
        public Stat Health { get; private set; }
        // koľko Update() zostáva, kým je ešte nezraniteľný - vykresluje sa semi transparentne
        public Stat InvicibilityRemaining { get; private set; }

        // startSector - kde sa objaví
        // tickPerSector - poćet Update kým prejde jeden sektor (= MovementSpeed)
        // baseHealth - aký má Health nazačiatku
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

        // Actor inkasuje zranenie, stane sa nachvíľu nezraniteľný
        public virtual void Damage(World world)
        {
            if (InvicibilityRemaining.Value == InvicibilityRemaining.MinValue)
            {
                Health.Decrease();
                InvicibilityRemaining.Reset();
            }
        }
    }
}
