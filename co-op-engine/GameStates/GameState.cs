using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace co_op_engine.GameStates
{
    public abstract class GameState
    {
        protected Game1 GameRef;

        public GameState(Game1 game)
        {
            this.GameRef = game;
        }

        public abstract void LoadContent();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime);
    }
}
