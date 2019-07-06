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
    class World
    {
        private Grid grid;
        private Actor charactor;

        public World(Texture2D texture)
        {
            charactor = new Actor(texture);
            grid = new Grid(3, 4, new List<Block> { Block.Floor, Block.Floor, Block.Brick, Block.Floor, Block.Wall, Block.Floor, Block.Floor, Block.Floor, Block.Wall, Block.Floor, Block.Floor, Block.Floor }, texture);
        }

        public void Update(KeyboardState keyboardState)
        {
            charactor.Update(keyboardState, grid);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            grid.Draw(spriteBatch, offset);
            charactor.Draw(spriteBatch, offset);
        }
    }
}
