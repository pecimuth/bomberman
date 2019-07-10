using Microsoft.Xna.Framework;
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
        private static readonly float moonScrollCoefficient = 1 / 3f;
        private static readonly float bgScrollCoefficient = 1 / 4f;
        private static readonly Point moonSize = new Point(32, 32);
        private static readonly Point moonOrigin = new Point(416, 0);
        private static readonly Vector2 moonOffset = new Vector2(450, 70);

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D backgroundTexture;
        Texture2D atlasTexture;
        World world;
        StatusBar statusBar;

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
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            backgroundTexture = Content.Load<Texture2D>("stars");
            atlasTexture = Content.Load<Texture2D>("atlas");
            LevelLoader levelLoader = LevelLoader.FromTextFile(@"Config\levels.txt");
            world = new World(atlasTexture, levelLoader, 1);
            statusBar = new StatusBar(atlasTexture);

            // TODO: use this.Content to load your game content here
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

            // TODO: Add your update logic here
            world.Update(Keyboard.GetState());
            statusBar.Update(world);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            DrawBackground();
            world.Draw(spriteBatch, MakeOffsetToCenterCharactor());
            statusBar.Draw(spriteBatch);

            base.Draw(gameTime);
        }

        private Vector2 MakeOffsetToCenterCharactor()
        {
            return (Viewport - AnimatedSprite.Size) / 2 - world.Charactor.Sprite.Location;
        }

        private void DrawBackground()
        {
            Vector2 scrollVector = MakeOffsetToCenterCharactor() * bgScrollCoefficient;
            Rectangle source = new Rectangle(scrollVector.ToPoint(), Viewport.ToPoint());
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null);
            spriteBatch.Draw(backgroundTexture, Vector2.Zero, source, Color.White);
            spriteBatch.End();

            Vector2 moonScrollVector = MakeOffsetToCenterCharactor() * moonScrollCoefficient;
            Rectangle moonSource = new Rectangle(moonOrigin, moonSize);
            Rectangle moonDestination = new Rectangle((moonScrollVector + moonOffset).ToPoint(), moonSize);
            spriteBatch.Begin();
            spriteBatch.Draw(atlasTexture, moonDestination, moonSource, Color.White);
            spriteBatch.End();
        }
    }
}
