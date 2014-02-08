using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Rendering
{
    public interface IRenderable
    {
        Texture2D Texture { get; }
        void Draw(SpriteBatch spriteBatch);
    }
}
