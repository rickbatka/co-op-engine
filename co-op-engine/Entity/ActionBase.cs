using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Entity
{
    /// <summary>
    /// houses the logic of the object, how it behaves thinks
    /// and makes decisions.  has all events relating to decision
    /// making and input.
    /// </summary>
    abstract class ActionBase
    {
        //movement direction vect
        //object state manager? (maybe make it generic enough for all game objects

        //events

        //constructor takes dataobject to populate values, could be aggregate like a 
        // higher level dataobject with a base dataobject component passed through

        abstract public void Update(Microsoft.Xna.Framework.GameTime gameTime);
        abstract public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch);
    }
}
