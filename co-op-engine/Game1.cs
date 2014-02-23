#region Using Statements
using co_op_engine.Components;
using co_op_engine.Components.Brains.AI;
using co_op_engine.ServiceProviders;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameExtensions;
using System.Collections.Generic;
using co_op_engine.Collections;
using co_op_engine.Factories;
using co_op_engine.World.Level;
using co_op_engine.GameStates;

#endregion

namespace co_op_engine
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        public Rectangle screenRectangle;
        const int gridSize = 32;

        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        private Texture2D plainWhiteTexture;
        private Texture2D towerTexture;
        private Texture2D arrowTexture;
        private Texture2D knightTexture;

        GameState CurrentGameState;

        public static Texture2D DEBUG_GRID_TEXTURE;

        public Game1()
            : base()
        {
            screenRectangle = new Rectangle(0,0,800,600);
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            Components.Add(new InputHandler(this));
            graphics.PreferredBackBufferHeight = screenRectangle.Height;
            graphics.PreferredBackBufferWidth = screenRectangle.Width;

            CurrentGameState = new GamePlay(this);
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

            CurrentGameState.LoadContent();
            DEBUG_GRID_TEXTURE = Content.Load<Texture2D>("grid");

            //plainWhiteTexture = Content.Load<Texture2D>("pixel");
            //plainWhiteTexture.SetData<Color>(new Color[] { Color.White });
            //arrowTexture = Content.Load<Texture2D>("arrow");
            //towerTexture = Content.Load<Texture2D>("tower");
            //knightTexture = Content.Load<Texture2D>("ww");

            //var animations = AnimationSet.BuildFromAsset("content/ww.txt");

            /////////////////////////////////////////////////////////////
            
            ////@TODO move to level setup
            //PlayerFactory.GetPlayer(container, knightTexture, animations);

            ////Players.Add(PlayerFactory.GetPlayer(this, tree, knightTexture, new TileSheet(animation)));

            //////@TODO EnemyFactory
            ////var devEnemy = new GameObject();
            //////devEnemy.SetBrain(new StepFollow(devEnemy));
            ////devEnemy.SetBrain(new DoNothingBrain(devEnemy));
            ////devEnemy.SetupDevTempComponents(arrowTexture, tree);
            ////Enemies.Add(devEnemy);

            //TowerFactory.GetDoNothingTower(container, towerTexture, plainWhiteTexture);
            
            
            ////var devTower = TowerFactory.GetDoNothingTower(this, tree, towerTexture, plainWhiteTexture);
            ////Towers.Add(devTower);


            this.Window.SetPosition(new Point(50, 50)); //moved it here cause it couldn't be moved if it was being updated every game loop
            
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
