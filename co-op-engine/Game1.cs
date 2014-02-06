#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using MonoGameExtensions;
using co_op_engine.Utility;

#endregion

namespace co_op_engine
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D plainWhiteTexture;
        Color defaultDrawingColor;
        int testVar = 1;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            plainWhiteTexture = new Texture2D(graphics.GraphicsDevice, 1, 1);
            plainWhiteTexture.SetData<Color>(new Color[] { Color.White });
            this.Window.SetPosition(new Point(0,0));
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            GameTimerManager.Instance.SetTimer(100, s => { 
                testVar = this.ChangeColor(false, 5);
                GameTimerManager.Instance.SetTimer(100, t => ChangeColor(true, 20));
            });

            GameTimerManager.Instance.SetTimer(300, t => ChangeColor(true, 20));
            GameTimerManager.Instance.SetTimer(400, t => ChangeColor(true, 20));
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            GameTimerManager.Instance.Update();

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
            spriteBatch.Draw(plainWhiteTexture, new Rectangle(10, 10, 10, 10), defaultDrawingColor);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public int ChangeColor(bool well, int huh)
        {
            if (defaultDrawingColor == Color.White)
            {
                defaultDrawingColor = Color.Black;
            }
            else
            {
                defaultDrawingColor = Color.White;
            }
            return 50;
        }
    }
}
