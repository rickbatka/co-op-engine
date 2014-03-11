using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using co_op_engine.UIElements;
using Microsoft.Xna.Framework;

namespace co_op_engine.GameStates
{
    class StartMenu : GameState
    {
        public StartMenu(Game1 game)
            : base(game)
        {
            controlManager.controls.Add(new Button(Assets
        }

        public override void LoadContent()
        {
            //load what is needed into assets
        }

        public override void Update(GameTime gameTime)
        {
            controlManager.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GameRef.spriteBatch.Begin();

            controlManager.Draw(GameRef.spriteBatch);

            GameRef.spriteBatch.End();
        }
    }
}
