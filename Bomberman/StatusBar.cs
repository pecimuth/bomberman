using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman
{
    class StatusBar
    {
        private static readonly Point bgTileSize = new Point(32, 32);
        private static readonly Point bgOrigin = new Point(384, 0);
        private static readonly Vector2 absoluteSpacer = new Vector2(32, 0);
        private static readonly Vector2 bombIconOffset = new Vector2(768, 0);
        private static readonly Vector2 iconOffset = new Vector2(4, 4);
        private static readonly Point iconSize = new Point(24, 24);
        private static readonly Point heartOutlineOrigin = new Point(0, 352);
        private static readonly Point heartOrigin = new Point(24, 352);
        private static readonly Point bombOrigin = new Point(48, 352);
        private static readonly int backgroundRepeat = 25;

        private Texture2D texture;
        private int outlines = 0;
        private int hearts = 0;
        private int bombs = 0;

        public StatusBar(Texture2D texture)
        {
            this.texture = texture;
        }

        public void Update(World world)
        {
            outlines = world.Charactor.Health.MaxValue;
            hearts = world.Charactor.Health.Value;
            bombs = world.Charactor.BombsAvailable;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            DrawBackground(spriteBatch);
            DrawIcons(spriteBatch, heartOutlineOrigin, absoluteSpacer, outlines, iconOffset);
            DrawIcons(spriteBatch, heartOrigin, absoluteSpacer, hearts, iconOffset);
            DrawIcons(spriteBatch, bombOrigin, -absoluteSpacer, bombs, iconOffset + bombIconOffset);
            spriteBatch.End();
        }
        
        private void DrawIcons(SpriteBatch spriteBatch, Point origin, Vector2 spacer, int count, Vector2 offset)
        {
            Rectangle source = new Rectangle(origin, iconSize);
            for (int i = 0; i < count; ++i)
            {
                Vector2 destinationVector = offset + i * spacer;
                Rectangle destination = new Rectangle(destinationVector.ToPoint(), iconSize);
                spriteBatch.Draw(texture, destination, source, Color.White);
            }
        }

        private void DrawBackground(SpriteBatch spriteBatch)
        {
            Rectangle source = new Rectangle(bgOrigin, bgTileSize);
            for (int i = 0; i < backgroundRepeat; ++i)
            {
                Rectangle destination = new Rectangle((i * absoluteSpacer).ToPoint(), bgTileSize);
                spriteBatch.Draw(texture, destination, source, Color.White);
            }
        }
    }
}
