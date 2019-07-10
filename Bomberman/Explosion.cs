﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman
{
    enum ExplosionOrientation
    {
        Vertical = 0,
        Horizontal = 1,
        Central = 2
    }

    class Explosion : Effect
    {
        private static readonly int ticksLeft = 30;
        private static readonly Vector2 originVector = new Vector2(32, 288);

        public Explosion(Texture2D texture, Sector location, ExplosionOrientation orientation) :
            base(
                 texture
                , false
                , location
                , ticksLeft
                , GetPointOfOrigin(orientation)
           )
        { }

        private static Point GetPointOfOrigin(ExplosionOrientation orientation)
        {
            Vector2 directionVector = new Vector2(Size.X, 0);
            Vector2 resultVector = originVector + (int)orientation * directionVector;
            return resultVector.ToPoint();
        }
    }
}
