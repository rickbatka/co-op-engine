using co_op_engine.Components.Movement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Brains
{
    abstract class BrainBase : ISentient
    {
        protected GameObject owner;

        public BrainBase(GameObject owner)
        {
            this.owner = owner;
        }

        abstract public void Update(GameTime gameTime);
        abstract public void Draw(SpriteBatch spriteBatch);

    }
}
