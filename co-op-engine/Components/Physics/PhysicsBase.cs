using co_op_engine.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace co_op_engine.Components.Physics
{
    /// <summary>
    /// contains the physical representation of the object
    /// in the level.  implementations can be physical or not.
    /// </summary>
    abstract class PhysicsBase : IPhysical
    {
        protected Rectangle boundingBox;
        public Rectangle BoundingBox { get { return boundingBox; } }

        protected GameObject owner;
        //velocity vect
        //max velocity scalar
        //force scalar
        //friction scalar
        //position vect
        //bounding box rect
        //facing direction (radial or enum, undecided) scalar/vect

        //events 

        //physicsBase( dataobject ) I like the concept of pairing large data driven classes with data objects loaded from editors

        public PhysicsBase(GameObject owner)
        {
            this.owner = owner;

            //@TOSO get position, width, height from player
            this.boundingBox = new Rectangle((int)owner.Position.X, (int)owner.Position.Y, owner.Width, owner.Height);
        }

        abstract public void Update(GameTime gameTime);
        abstract public void Draw(SpriteBatch spriteBatch);
    }
}
