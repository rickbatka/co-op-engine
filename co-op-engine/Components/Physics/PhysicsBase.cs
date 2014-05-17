using co_op_engine.Collections;
using co_op_engine.Components;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

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
        //UNITS!!!
        //100px is 1 meter
        //time is in seconds

        protected float friction = 0.5f;
        protected float speedLimit
        {
            get
            {
                if (owner != null && owner.CurrentStateProperties.IsBoosting)
                {
                    return 500f;
                }
                return 150f;
            }
        }

        protected float accelerationModifier
        {
            get
            {
                if (owner != null && owner.CurrentStateProperties.IsBoosting)
                {
                    return 2*owner.SpeedAccel;
                }
                return owner.SpeedAccel;
            }
        }

        protected GameObject owner;

        public PhysicsBase(GameObject owner)
        {
            this.owner = owner;
            this.owner.FacingDirection = Constants.South;            
        }

        virtual public void Update(GameTime gameTime)
        {
            if (owner.InputMovementVector.Length() > 0) owner.InputMovementVector.Normalize();
            owner.Acceleration = (owner.InputMovementVector * accelerationModifier);

            owner.Velocity *= friction;

            //if ((owner.Velocity + owner.Acceleration).Length() < speedLimit)
            //{
                owner.Velocity += owner.Acceleration;
            /*}
            else
            {
#warning quick fix, this was a bad setup before leading to only accellerating if < speed, needs in betweens
                owner.Velocity += owner.Acceleration;
                owner.Velocity.Normalize();
                owner.Velocity *= speedLimit;
            }*/

            owner.Acceleration = Vector2.Zero;

            var newPosition = owner.Position + (owner.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds);
            owner.Position = LockToLevel(newPosition); // we can't ever let a player out of the level. ever.
            VerifyBoundingBox();
            SetFacingDirection();
        }

        private Vector2 LockToLevel(Vector2 position)
        {
            var lockedX = MathHelper.Clamp(
                position.X, 
                owner.CurrentLevel.Bounds.Left + owner.BoundingBox.Width / 2, 
                owner.CurrentLevel.Bounds.Right - owner.BoundingBox.Width / 2
            );

            var lockedY = MathHelper.Clamp(
                position.Y, 
                owner.CurrentLevel.Bounds.Top + owner.BoundingBox.Height / 2, 
                owner.CurrentLevel.Bounds.Bottom - owner.BoundingBox.Height / 2
            );

            return new Vector2(lockedX, lockedY);
        }

        public void VerifyBoundingBox()
        {
            owner.BoundingBox = new Rectangle(
                (int)(owner.CurrentFrame.PhysicsRectangle.X + (this.owner.Position.X - this.owner.CurrentFrame.DrawRectangle.Center.X)),
                (int)(owner.CurrentFrame.PhysicsRectangle.Y + (this.owner.Position.Y - this.owner.CurrentFrame.DrawRectangle.Center.Y)), 
                this.owner.CurrentFrame.PhysicsRectangle.Width, 
                this.owner.CurrentFrame.PhysicsRectangle.Height);
        }

        private void SetFacingDirection()
        {
            int oldDirection = owner.FacingDirection;
            int newDirection = oldDirection;

            var rotation = owner.RotationTowardFacingDirectionRadians;

            if (Math.Abs(rotation) < (Math.PI) / 3f)
            {
                newDirection = Constants.North;
            }
            else if (Math.Abs(rotation) > ((Math.PI) / 3f) * 2)
            {
                newDirection = Constants.South;
            }
            else
            {
                if (rotation < 0)
                {
                    newDirection = Constants.West;
                }
                else
                {
                    newDirection = Constants.East;
                }
            }

            owner.FacingDirection = newDirection;
        }

        virtual public void Draw(SpriteBatch spriteBatch) { }

        virtual public void DebugDraw(SpriteBatch spriteBatch) { }

        internal void ReceiveCommand(Networking.Commands.GameObjectCommand command)
        {
            throw new NotImplementedException();
        }
    }
}


/* * * * * *
 * notes on physics overhaul:
 *  - units units units
 *  - only use friction and force
 *  - introduce concept of impulse force along with it's own friction coefficient
 *  - 
 */