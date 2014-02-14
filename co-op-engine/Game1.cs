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

#endregion

namespace co_op_engine
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game, IActorInformationProvider
    {
        Rectangle screenRectangle;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        List<GameObject> Players = new List<GameObject>();
        List<GameObject> Enemies = new List<GameObject>();
        GameObject devPlayerObject;
        
        ElasticQuadTree tree;
        Texture2D devOutline;

        public Game1()
            : base()
        {
            screenRectangle = new Rectangle(0,0,512,512);

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
            
            var plainWhiteTexture = new Texture2D(graphics.GraphicsDevice, 1, 1);
            plainWhiteTexture.SetData<Color>(new Color[] { Color.White });
            devPlayerObject = new GameObject();
            devPlayerObject.SetupDevTempComponents(plainWhiteTexture, tree);
            Players.Add(devPlayerObject);

            var devEnemy = new GameObject();
            devEnemy.SetBrain(new StepFollow(devEnemy));
            devEnemy.SetupDevTempComponents(plainWhiteTexture, tree);
            Enemies.Add(devEnemy);

            ///////////////////////////////////////////////////////////
            
            base.Initialize();
        }

        private void RegisterServices()
        {
            GameServicesProvider.AddService(typeof(IActorInformationProvider), this);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            devOutline = new Texture2D(graphics.GraphicsDevice, 1, 1);
            devOutline.SetData<Color>(new Color[] { Color.White });

            // TODO: use this.Content to load your game content here

            var animation = AnimatedRectangle.BuildFromAsset(@"content/exampleAnimationMetaData.txt");
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

            tree.Draw(spriteBatch, devOutline);

            foreach (var player in Players)
            {
                player.Draw(spriteBatch);
            }

            foreach (var enemy in Enemies)
            {
                enemy.Draw(spriteBatch);
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

    }
}
