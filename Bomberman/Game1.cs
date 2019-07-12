using Bomberman.Parser;
using Bomberman.UI;
using Bomberman.World;
using Bomberman.World.Actors.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;

namespace Bomberman
{
    public class Game1 : Game
    {
        // číslo levelu ktorý sa zobrazí ako prvý
        static readonly int firstLevel = 1;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Audio audio;
        Texture2D backgroundTexture;
        Texture2D atlasTexture;
        SpriteFont font;
        Background background;
        World.World world;
        Texts texts;
        StatusBar statusBar;
        LevelLoader levelLoader;

        // rozmery zobrazovaného okna
        Vector2 Viewport
        {
            get
            {
                return new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            }
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        // načítanie audia, textúr, fontu, levelov, vytvorenie inštancie sveta, status baru, pozadia
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            audio = new Audio();
            audio.Register(Sound.Drop, Content.Load<SoundEffect>("drop"));
            audio.Register(Sound.Start, Content.Load<SoundEffect>("start"));
            audio.Register(Sound.Explosion, Content.Load<SoundEffect>("explosion"));
            audio.Register(Sound.Hurt, Content.Load<SoundEffect>("hurt"));
            audio.Register(Sound.Pickup, Content.Load<SoundEffect>("pickup"));
            spriteBatch = new SpriteBatch(GraphicsDevice);
            backgroundTexture = Content.Load<Texture2D>("stars");
            atlasTexture = Content.Load<Texture2D>("atlas");
            levelLoader = LevelLoader.FromTextFile(@"Config\levels.txt");
            font = Content.Load<SpriteFont>("font");
            world = new World.World(audio, atlasTexture, levelLoader, firstLevel);
            texts = levelLoader.GetTexts(world.LevelNumber);
            statusBar = new StatusBar(atlasTexture);
            background = new Background(backgroundTexture, atlasTexture);
        }

        protected override void UnloadContent()
        { }

        // herná logika - načítanie vstupov, animácie, pohyb, audio...
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (world.LevelState == LevelState.InProgress)
            {
                world.Update(Keyboard.GetState());
                statusBar.Update(world);
            }
            else if (world.LevelState == LevelState.Completed)
            {
                int nextLevel = world.LevelNumber + 1;
                if (nextLevel > levelLoader.LevelCount())
                {
                    nextLevel = firstLevel;
                }
                world = new World.World(audio, atlasTexture, levelLoader, nextLevel);
                texts = levelLoader.GetTexts(world.LevelNumber);
                world.Update(Keyboard.GetState());
                statusBar.Update(world);
            }
            else if (world.LevelState == LevelState.Failed)
            {
                world = new World.World(audio, atlasTexture, levelLoader, world.LevelNumber);
                texts = levelLoader.GetTexts(world.LevelNumber);
                world.Update(Keyboard.GetState());
                statusBar.Update(world);
            }

            base.Update(gameTime);
        }

        // vykreslenie hry
        protected override void Draw(GameTime gameTime)
        {
            Vector2 charactorOffset = MakeOffsetToCenterCharactor();
            background.Draw(spriteBatch, charactorOffset, Viewport);
            world.Draw(spriteBatch, charactorOffset);
            texts.Draw(spriteBatch, font, charactorOffset);
            statusBar.Draw(spriteBatch);

            base.Draw(gameTime);
        }

        // vypočíta s akým posunutím sa má vykresliť Charactor aby bol vycentrovaný na stred okna
        // s týmto posunutím sa vykreslí celý svet (World)
        // podľa toho sa potom počíta aj posunutie pozadia (vynásobenie nejakým koeficientom <1)
        private Vector2 MakeOffsetToCenterCharactor()
        {
            return (Viewport - AnimatedSprite.Size) / 2 - world.Charactor.Sprite.Location;
        }
    }
}
