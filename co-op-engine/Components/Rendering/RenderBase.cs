using co_op_engine.Components.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Rendering
{
    class RenderBase : IRenderable
    {
        protected Texture2D texture;
        public Texture2D Texture { get { return texture; } }

        protected readonly GameObject owner;

        protected Rectangle? currentDrawRectangle = null;

        public RenderBase(GameObject owner, Texture2D texture)
        {
            this.owner = owner;
            this.texture = texture;
        }

        virtual public void Update(GameTime gameTime)
        { 
            
        }

        virtual public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, GetDrawTarget(), currentDrawRectangle, Color.White);
        }

        private Rectangle GetDrawTarget()
        {
            return new Rectangle(
                x: (int)owner.Position.X,
                y: (int)owner.Position.Y,
                width: owner.Width,
                height: owner.Height
            );
        }
    }
}
