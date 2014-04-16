using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameExtensions;
using co_op_engine.GameStates;


namespace co_op_engine
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        public Rectangle screenRectangle;
        public int gridSize = 32;

        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        GameState CurrentGameState;

        public Game1()
            : base()
        {
            screenRectangle = new Rectangle(0,0,1024,768);
            graphics = new GraphicsDeviceManager(this);


            Content.RootDirectory = "Content";
            Components.Add(new InputHandler(this));
            //graphics.PreferredBackBufferHeight = screenRectangle.Height;
            //5graphics.PreferredBackBufferWidth = screenRectangle.Width;

            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = screenRectangle.Width;
            graphics.PreferredBackBufferHeight = screenRectangle.Height;
            this.Window.SetPosition(screenRectangle.Location); //moved it here cause it couldn't be moved if it was being updated every game loop

        }

        public void ChangeGameState(GameState state)
        {
            state.LoadContent();
            CurrentGameState = state;
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
            // Create a new SpriteBatch, load textures
            spriteBatch = new SpriteBatch(GraphicsDevice);
            AssetRepository.Initialize(this);

            CurrentGameState = new StartMenu(this);
            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            if(InputHandler.ButtonPressed(Buttons.Back, PlayerIndex.One) || InputHandler.KeyPressed(Keys.Escape))
                Exit();
            GameTimerManager.Instance.Update(gameTime);

            //container.UpdateAll(gameTime);

            CurrentGameState.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            CurrentGameState.Draw(gameTime);

            //spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointWrap, null, null);

            //container.DrawAll(spriteBatch);
            //container.DebugDraw(spriteBatch, DEBUG_GRID_TEXTURE);



            //spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
