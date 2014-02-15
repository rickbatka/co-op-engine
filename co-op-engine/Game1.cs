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

#endregion

namespace co_op_engine
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game, IActorInformationProvider, IGraphicsInformationProvider
    {
        Rectangle screenRectangle;
        const int gridSize = 32;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Texture2D plainWhiteTexture;
        private Texture2D towerTexture;
        private Texture2D arrowTexture;
        private Texture2D knightTexture;

        List<GameObject> Players = new List<GameObject>();
        List<GameObject> Enemies = new List<GameObject>();
        List<GameObject> Towers = new List<GameObject>();
        
        ElasticQuadTree tree;

        public Game1()
            : base()
        {
            screenRectangle = new Rectangle(0,0,800,600);

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Components.Add(new InputHandler(this));
            graphics.PreferredBackBufferHeight = screenRectangle.Height;
            graphics.PreferredBackBufferWidth = screenRectangle.Width;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            GameServicesProvider.Install(this);
            RegisterServices();

            ///////////////////////////////////////////////////////////
            // @TODO move to factory
            tree = new ElasticQuadTree(RectangleFloat.FromRectangle(screenRectangle), null);

            ///////////////////////////////////////////////////////////
            
            base.Initialize();
        }

        private void RegisterServices()
        {
            GameServicesProvider.AddService(typeof(IActorInformationProvider), this);
            GameServicesProvider.AddService(typeof(IGraphicsInformationProvider), this);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, load textures
            spriteBatch = new SpriteBatch(GraphicsDevice);

            plainWhiteTexture = Content.Load<Texture2D>("pixel");
            plainWhiteTexture.SetData<Color>(new Color[] { Color.White });
            arrowTexture = Content.Load<Texture2D>("arrow");
            towerTexture = Content.Load<Texture2D>("tower");
            knightTexture = Content.Load<Texture2D>("knightsheet");
            var animation = AnimatedRectangle.BuildFromAsset(@"content/exampleAnimationMetaData.txt");

            ///////////////////////////////////////////////////////////
            
            //@TODO move to level setup
            Players.Add(PlayerFactory.GetPlayer(this, tree, knightTexture, new TileSheet(animation)));

            //@TODO EnemyFactory
            var devEnemy = new GameObject();
            devEnemy.SetBrain(new StepFollow(devEnemy));
            devEnemy.SetupDevTempComponents(arrowTexture, tree);
            Enemies.Add(devEnemy);

            
            var devTower = TowerFactory.GetDoNothingTower(this, tree, towerTexture, plainWhiteTexture);
            Towers.Add(devTower);

            

            
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

            foreach (var player in Players)
            {
                player.Update(gameTime);
            }

            foreach (var enemy in Enemies)
            {
                enemy.Update(gameTime);
            }

            foreach (var tower in Towers)
            {
                tower.Update(gameTime);
            }

            //@TODO FIX for some reason I have to have this here or my window is way off the top left. I think due to my resolution / scaling.
            this.Window.SetPosition(new Point(50, 50));

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            //tree.Draw(spriteBatch, devOutline);

            foreach (var player in Players)
            {
                player.Draw(spriteBatch);
            }

            foreach (var enemy in Enemies)
            {
                enemy.Draw(spriteBatch);
            }

            foreach (var tower in Towers)
            {
                tower.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        #region IActorInformationProvider

        public List<GameObject> GetPlayers()
        {
            return Players;
        }

        #endregion

        #region IGraphicsInformationProvider

        public Rectangle ScreenRectangle { get { return screenRectangle; } }
        public int GridSize { get { return gridSize; } }

        #endregion

    }
}
