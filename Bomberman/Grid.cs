using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman
{
    enum Block
    {
        None = -1,
        Floor = 0,
        Brick = 1,
        Wall = 2
    }

    class Grid
    {
        public int Width { get; }
        public int Height { get; }
        private List<Block> blocks;
        public Texture2D Texture;
        private static Vector2 textureOffset = new Vector2(0, 8 * 32);

        public Grid(int width, int height, List<Block> blocks)
        {
            Width = width;
            Height = height;
            this.blocks = blocks;
        }
 
        public Block At(Sector sector)
        {
            return blocks[sector.X + sector.Y * Width];
        }

        public Block At(int x, int y)
        {
            return At(new Sector(x, y));
        }

        public bool Contains(Sector sector)
        {
            return sector.X >= 0 && sector.X < Width && sector.Y >= 0 && sector.Y < Height;
        }

        public bool IsFloor(Sector sector)
        {
            return Contains(sector) && At(sector) == Block.Floor;
        }

        public bool Break(Sector sector)
        {
            Block target = At(sector);
            if (target == Block.Brick)
            {
                Set(sector, Block.Floor);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            spriteBatch.Begin();
            for (int y = 0; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    Sector sector = new Sector(x, y);
                    Block block = At(sector);

                    if (block == Block.None)
                    {
                        continue;
                    }

                    Rectangle source = MakeSourceRectangle(block);
                    Point location = (sector.ToVector() + offset).ToPoint();
                    Rectangle destination = new Rectangle(location, Sector.Size.ToPoint());
                    
                    spriteBatch.Draw(Texture, destination, source, Color.White);
                }
            }
            spriteBatch.End();
        }

        private Rectangle MakeSourceRectangle(Block block)
        {
            Vector2 originVector = Sector.Size * new Vector2((int)block, 0) + textureOffset;
            Point origin = originVector.ToPoint();
            Rectangle rectangle = new Rectangle(origin, Sector.Size.ToPoint());
            return rectangle;
        }

        private void Set(Sector sector, Block block)
        {
            blocks[sector.X + sector.Y * Width] = block;
        }
    }
}
