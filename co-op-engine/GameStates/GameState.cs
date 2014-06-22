using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using co_op_engine.UIElements;
using Microsoft.Xna.Framework;

namespace co_op_engine.GameStates
{
    public abstract class GameState
    {
        protected Game1 GameRef;

        public ControlManager controlManager;

        public GameState(Game1 game)
        {
            this.GameRef = game;
            controlManager = new ControlManager();
        }

        public abstract void LoadContent();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime);
    }
}
