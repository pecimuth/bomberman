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
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
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

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
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

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
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

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Vector2 charactorOffset = MakeOffsetToCenterCharactor();
            background.Draw(spriteBatch, charactorOffset, Viewport);
            world.Draw(spriteBatch, charactorOffset);
            texts.Draw(spriteBatch, font, charactorOffset);
            statusBar.Draw(spriteBatch);

            base.Draw(gameTime);
        }

        private Vector2 MakeOffsetToCenterCharactor()
        {
            return (Viewport - AnimatedSprite.Size) / 2 - world.Charactor.Sprite.Location;
        }
    }
}
