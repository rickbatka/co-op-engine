using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Engines
{
    public class EngineBase
    {
        protected GameObject Owner;
        
        public EngineBase(GameObject owner)
        {
            this.Owner = owner;
        }

        virtual public void Draw(SpriteBatch spriteBatch) { }

        virtual public void DebugDraw(SpriteBatch spriteBatch) { }

        virtual public void Update(GameTime gameTime) { }
    }
}
