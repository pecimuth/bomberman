using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bomberman.UI
{
    class StatusBar
    {
        // pozadie status baru
        // veľkosť dlaždice
        private static readonly Point bgTileSize = new Point(32, 32);
        // kde sa nachádza v atlase
        private static readonly Point bgOrigin = new Point(384, 0);
        // miesto medzi ikonkami (absolútna hodnota)
        private static readonly Vector2 absoluteSpacer = new Vector2(32, 0);
        // posunutie prvej ikonky bomby
        private static readonly Vector2 bombIconOffset = new Vector2(768, 0);
        // posunutie prvej ikonky srdca
        private static readonly Vector2 iconOffset = new Vector2(4, 4);
        private static readonly Point iconSize = new Point(24, 24);
        // kde sa nachádzajú ikonky v atlase
        private static readonly Point heartOutlineOrigin = new Point(0, 352);
        private static readonly Point heartOrigin = new Point(24, 352);
        private static readonly Point bombOrigin = new Point(48, 352);
        // koľkokrát nakresliť dlaždicu pozadia
        private static readonly int backgroundRepeat = 25;

        private Texture2D texture;
        // počty jednotlivých ikoniek na vykreslenie
        private int outlines = 0;
        private int hearts = 0;
        private int bombs = 0;

        // texture je atlas
        public StatusBar(Texture2D texture)
        {
            this.texture = texture;
        }

        // aktualizujú sa poćty ikoniek na vykreslienie
        public void Update(World.World world)
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

        // nakresli jeden typ ikonky v atlase na mieste origin, s medzerami veľkosti spacer, count-krát s posunutím offset
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

        // vykresli dlaždice pozadia
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
