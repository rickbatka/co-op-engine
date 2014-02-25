using co_op_engine.Components.Rendering;
using co_op_engine.Content;
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
        protected readonly IRenderable owner;

        protected Rectangle? currentDrawRectangle = null;

        public RenderBase(IRenderable owner, Texture2D texture)
        {
            this.owner = owner;
            this.owner.TextureProp = texture;
        }

        virtual public void Update(GameTime gameTime)
        { 
            
        }

        virtual public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(owner.TextureProp, GetDrawTarget(), currentDrawRectangle, Color.White);
        }

        virtual public void DebugDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetRepository.Instance.PlainWhiteTexture, GetDrawTarget(), currentDrawRectangle, Color.White);
        }

        private Rectangle GetDrawTarget()
        {
            return new Rectangle(
                x: (int)(owner.PositionProp.X - owner.WidthProp/2),
                y: (int)(owner.PositionProp.Y - owner.HeightProp/2),
                width: owner.WidthProp,
                height: owner.HeightProp
            );
        }
    }
}
