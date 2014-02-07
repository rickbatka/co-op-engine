using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Entity
{
    /// <summary>
    /// contains the physical representation of the object
    /// in the level.  implementations can be physical or not.
    /// </summary>
    abstract class PhysicsBase
    {
        //velocity vect
        //max velocity scalar
        //force scalar
        //friction scalar
        //position vect
        //bounding box rect
        //facing direction (radial or enum, undecided) scalar/vect

        //events 

        //physicsBase( dataobject ) I like the concept of pairing large data driven classes with data objects loaded from editors
        
        abstract public void Update(Microsoft.Xna.Framework.GameTime gameTime);
        abstract public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch);
    }
}
