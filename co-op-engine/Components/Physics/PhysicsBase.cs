using co_op_engine.Collections;
using co_op_engine.Components;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace co_op_engine.Components.Physics
{
    public delegate void ActorDirectionChangedEventHandler (PhysicsBase sender, ActorDirectionChangedEventArgs directionData);
    public struct ActorDirectionChangedEventArgs
    {
        public int OldDirection;
        public int NewDirection;
    }
    /// <summary>
    /// contains the physical representation of the object
    /// in the level.  implementations can be physical or not.
    /// </summary>
    public class PhysicsBase
    {
        protected float friction = 0.5f;
        protected float speedLimit = 150f;
        protected float accelerationModifier = 400f;
        protected float boostingModifier = 1.5f;

        public event ActorDirectionChangedEventHandler OnActorDirectionChanged;

        protected GameObject owner;

        public PhysicsBase(GameObject owner)
        {
            this.owner = owner;
            this.owner.FacingDirection = Constants.South;            
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
            VerifyBoundingBox();
            SetFacingDirection();
        }

        public void VerifyBoundingBox()
        {
            owner.BoundingBox = new Rectangle((int)(this.owner.Position.X - this.owner.BoundingBox.Width / 2), (int)(this.owner.Position.Y - this.owner.BoundingBox.Height / 2), this.owner.Width, this.owner.Height);
        }

        private void SetFacingDirection()
        {
            int oldDirection = owner.FacingDirection;
            int newDirection = oldDirection;

            if (owner.InputMovementVector.X == 0 && owner.InputMovementVector.Y == 0)
            {
                return;
            }

            float movementX = Math.Abs(owner.InputMovementVector.X);
            float movementY = Math.Abs(owner.InputMovementVector.Y);
            
            if (movementY >= movementX)
            {
                if (owner.InputMovementVector.Y < 0)
                {
                    newDirection = Constants.North;
                }
                else 
                {
                    newDirection = Constants.South;
                }
            }
            else
            {
                if (owner.InputMovementVector.X < 0)
                {
                    newDirection = Constants.West;
                }
                else 
                {
                    newDirection = Constants.East;    
                }
            }
            owner.FacingDirection = newDirection;

            if (newDirection != oldDirection && OnActorDirectionChanged != null)
            {
                OnActorDirectionChanged(this, new ActorDirectionChangedEventArgs() { OldDirection = oldDirection, NewDirection = newDirection });
            }
        }

        virtual public void Draw(SpriteBatch spriteBatch) { }

        virtual public void DebugDraw(SpriteBatch spriteBatch) { }
    }
}
