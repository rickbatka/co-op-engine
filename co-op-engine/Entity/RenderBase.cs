using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Entity
{
    /// <summary>
    /// contains all the needed information for drawing
    /// a game object. implemented classes could animate
    /// or have state machines to change behavior
    /// </summary>
    abstract class RenderBase
    {
        //texture
        //owner reference

        //events

        //another candidate for the component data object system

        abstract public void Update(Microsoft.Xna.Framework.GameTime gameTime);
        abstract public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch);
    }
}
