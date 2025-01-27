﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman.World.Grids
{
    enum Facing
    {
        South = 0,
        West = 1,
        East = 2,
        North = 3
    }

    struct Sector
    {
        public readonly static Sector Zero = new Sector(0, 0);
        public readonly static Sector One = new Sector(1, 1);
        // veľkosť sektoru
        public readonly static int Width = 32;
        public readonly static int Height = 32;

        public static Vector2 Size
        {
            get
            {
                return new Vector2(Width, Height);
            }
        }

        // súradnice sektoru, indeované od 0
        public int X;
        public int Y;

        public Sector(int x, int y)
        {
            X = x;
            Y = y;
        }

        // vytvorí sa sektor, do ktorého patrí daný bod
        public static Sector FromVector(Vector2 vector)
        {
            return new Sector((int)vector.X / Width, (int)vector.Y / Height);
        }

        // vráti ľavý horný roh sektoru
        public Vector2 ToVector()
        {
            return Size * new Vector2(X, Y);
        }

        // vráti susedný sektor podľa orientácie
        public Sector Neighbor(Facing facing)
        {
            switch (facing)
            {
                case Facing.North:
                    return new Sector(X, Y - 1);
                case Facing.East:
                    return new Sector(X + 1, Y);
                case Facing.West:
                    return new Sector(X - 1, Y);
                case Facing.South:
                    return new Sector(X, Y + 1);
                default:
                    return new Sector(X, Y);
            }
        }

        // rovnosť poďla zložiek
        public static bool operator ==(Sector right, Sector left)
        {
            return right.X == left.X && right.Y == left.Y;
        }

        // nerovnosť po zložkách
        public static bool operator !=(Sector right, Sector left)
        {
            return !(right == left);
        }
    }
}
