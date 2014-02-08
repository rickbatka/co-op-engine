using co_op_engine.Components.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Rendering
{
    abstract class RenderBase : IRenderable
    {
        protected Texture2D _texture;
        public Texture2D Texture { get { return _texture; } }

        protected readonly GameObject owner;

        public RenderBase(GameObject owner)
        {
            this.owner = owner;
        }

        abstract public void Update(GameTime gameTime);
        abstract public void Draw(SpriteBatch spriteBatch);
    }
}
