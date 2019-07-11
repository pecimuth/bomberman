using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman
{
    class Background
    {
        private static readonly float moonScrollCoefficient = 1 / 7f;
        private static readonly float bgScrollCoefficient = 1 / 10f;
        private static readonly Point moonSize = new Point(32, 32);
        private static readonly Point moonOrigin = new Point(416, 0);
        private static readonly Vector2 moonOffset = new Vector2(450, 70);
        private readonly Texture2D backgroundTexture;
        private readonly Texture2D atlasTexture;

        public Background(Texture2D backgroundTexture, Texture2D atlasTexture)
        {
            this.backgroundTexture = backgroundTexture;
            this.atlasTexture = atlasTexture;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 charactorOffset, Vector2 viewport)
        {
            Vector2 scrollVector = charactorOffset * bgScrollCoefficient;
            Rectangle source = new Rectangle(scrollVector.ToPoint(), viewport.ToPoint());
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null);
            spriteBatch.Draw(backgroundTexture, Vector2.Zero, source, Color.White);
            spriteBatch.End();

            Vector2 moonScrollVector = charactorOffset * moonScrollCoefficient;
            Rectangle moonSource = new Rectangle(moonOrigin, moonSize);
            Rectangle moonDestination = new Rectangle((moonScrollVector + moonOffset).ToPoint(), moonSize);
            spriteBatch.Begin();
            spriteBatch.Draw(atlasTexture, moonDestination, moonSource, Color.White);
            spriteBatch.End();
        }
    }
}
