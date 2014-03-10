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
        protected float friction = 0.5f;
        protected float speedLimit = 150f;
        protected float accelerationModifier = 400f;
        protected float boostingModifier = 1.5f;

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

        virtual public void HandleCollision(List<GameObject> collidors) { }

        virtual public void Draw(SpriteBatch spriteBatch) { }

        virtual public void DebugDraw(SpriteBatch spriteBatch) 
        { 
            spriteBatch.Draw(AssetRepository.Instance.DebugGridTexture, this.owner.BoundingBox, Color.White);

            // object debug info
            spriteBatch.DrawString(
                spriteFont: AssetRepository.Instance.Arial,
                text: owner.Health + "/" + owner.MaxHealth,
                position: PositionAboveHead(25),
                color: Color.White
            );

            spriteBatch.DrawString(
                spriteFont: AssetRepository.Instance.Arial,
                text: owner.DisplayName,
                position: PositionAboveHead(50),
                color: Color.White
            );
        }

        private Vector2 PositionAboveHead(int distance)
        {
            var aboveHead = new Vector2(
                x: owner.Position.X - (owner.Width / 2f),
                y: owner.Position.Y - (owner.Height / 2f) - distance
            );

            return aboveHead;
        }
    }
}
