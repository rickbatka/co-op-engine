using co_op_engine.Collections;
using co_op_engine.Components;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace co_op_engine.Components.Physics
{
    /// <summary>
    /// contains the physical representation of the object
    /// in the level.  implementations can be physical or not.
    /// </summary>
    public class PhysicsBase
    {
        protected float friction = 0.5f;
        protected float speedLimit = 200f;
        protected float accelerationModifier = 400f;
        protected float boostingModifier = 1.5f;

        protected GameObject owner;

        public PhysicsBase(GameObject owner)
        {
            this.owner = owner;

            /////////////////////////////////////////
            //@TODO set these up in factory probably
            this.owner.Position = new Vector2(MechanicSingleton.Instance.rand.Next(1, 500));
            this.owner.Width = 50;
            this.owner.Height = 50;
            //@END temp setup code
            /////////////////////////////////////////

            //@TODO get position, width, height from player
            this.owner.BoundingBox = new Rectangle((int)(this.owner.Position.X - this.owner.BoundingBox.Width / 2), (int)(this.owner.Position.Y - this.owner.BoundingBox.Height / 2), this.owner.Width, this.owner.Height);
        }

        virtual public void Update(GameTime gameTime)
        {
            owner.Acceleration = (owner.InputMovementVector * accelerationModifier);

            owner.Velocity *= friction;

            if ((owner.Velocity + owner.Acceleration).Length() < speedLimit)
            {
                owner.Velocity += owner.Acceleration;
            }
            else
            {
#warning quick fix, this was a bad setup before leading to only accellerating if < speed, needs in betweens
                owner.Velocity += owner.Acceleration;
                owner.Velocity.Normalize();
                owner.Velocity *= speedLimit;
            }

            owner.Acceleration = Vector2.Zero;

            owner.Position += owner.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        virtual public void Draw(SpriteBatch spriteBatch) { }
    }
}
