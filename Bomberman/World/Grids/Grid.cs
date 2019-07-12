using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman.World.Grids
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
        // šírka mriežky
        public int Width { get; }
        // výška mriežky
        public int Height { get; }
        // bloky najprv prvý riadok zľava doprava, potom druhý riadok atď
        private List<Block> blocks;
        // atlas
        public Texture2D Texture;
        // ľavý horný roh Block.Floor v atlase
        private static Vector2 textureOffset = new Vector2(0, 8 * 32);

        public Grid(int width, int height, List<Block> blocks)
        {
            Width = width;
            Height = height;
            this.blocks = blocks;
        }

        // čo sa nachádza v danom sektore
        public Block At(Sector sector)
        {
            return blocks[sector.X + sector.Y * Width];
        }

        // čo sa nachádza v danom sektore - zadané x, y sektoru
        public Block At(int x, int y)
        {
            return At(new Sector(x, y));
        }

        // mriežka obsahuje daný sektor?
        public bool Contains(Sector sector)
        {
            return sector.X >= 0 && sector.X < Width && sector.Y >= 0 && sector.Y < Height;
        }

        // je na sektore Block.Floor?
        public bool IsFloor(Sector sector)
        {
            return Contains(sector) && At(sector) == Block.Floor;
        }

        // rozbitie tehál na sektore
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

        // vykreslenie mriežky s posunom
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

        // obdĺžnik v atlase, z ktorého sa daný typ bloku vykresluje
        private Rectangle MakeSourceRectangle(Block block)
        {
            Vector2 originVector = Sector.Size * new Vector2((int)block, 0) + textureOffset;
            Point origin = originVector.ToPoint();
            Rectangle rectangle = new Rectangle(origin, Sector.Size.ToPoint());
            return rectangle;
        }

        // zmena typu bloku v danom sektore
        private void Set(Sector sector, Block block)
        {
            blocks[sector.X + sector.Y * Width] = block;
        }
    }
}
