using co_op_engine.Components.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Rendering
{
    public class RenderBase
    {
        protected readonly GameObject owner;

        protected Rectangle? currentDrawRectangle = null;

        public RenderBase(GameObject owner, Texture2D texture)
        {
            this.owner = owner;
            this.owner.Texture = texture;
        }

        virtual public void Update(GameTime gameTime)
        { 
            
        }

        virtual public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(owner.Texture, GetDrawTarget(), currentDrawRectangle, Color.White);
        }

        private Rectangle GetDrawTarget()
        {
            return new Rectangle(
                x: (int)(owner.Position.X - owner.Width/2),
                y: (int)(owner.Position.Y - owner.Height/2),
                width: owner.Width,
                height: owner.Height
            );
        }
    }
}
