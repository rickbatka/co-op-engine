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

#endregion

namespace co_op_engine
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game, IActorInformationProvider
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        List<GameObject> Players = new List<GameObject>();
        List<GameObject> Enemies = new List<GameObject>();
        GameObject devPlayerObject;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Components.Add(new InputHandler(this));
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
            var plainWhiteTexture = new Texture2D(graphics.GraphicsDevice, 1, 1);
            plainWhiteTexture.SetData<Color>(new Color[] { Color.White });
            devPlayerObject = new GameObject();
            devPlayerObject.SetupDevTempComponents(plainWhiteTexture);
            Players.Add(devPlayerObject);

            var devEnemy = new GameObject();
            devEnemy.SetBrain(new StepFollow(devEnemy));
            devEnemy.SetupDevTempComponents(plainWhiteTexture);
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

            // TODO: use this.Content to load your game content here
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
