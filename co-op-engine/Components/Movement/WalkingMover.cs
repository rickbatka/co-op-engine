using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Movement
{
    public class WalkingMover : MoverBase
    {
        protected float accelerationModifier
        {
            get
            {
                if (Owner != null && Owner.CurrentStateProperties.IsBoosting)
                {
                    return Owner.BoostModifier * Owner.SpeedAccel;
                }
                return Owner.SpeedAccel;
            }
        }

        public WalkingMover(GameObject owner)
            :base(owner)
        { }

        public override void Update(GameTime gameTime)
        {
            if (Owner.InputMovementVector.Length() > 0) Owner.InputMovementVector.Normalize();
            Owner.Acceleration = (Owner.InputMovementVector * accelerationModifier);

            Owner.Velocity *= friction;

            //if ((Owner.Velocity + Owner.Acceleration).Length() < speedLimit)
            //{
            Owner.Velocity += Owner.Acceleration;
            /*}
            else
            {
#warning quick fix, this was a bad setup before leading to only accellerating if < speed, needs in betweens
                Owner.Velocity += Owner.Acceleration;
                Owner.Velocity.Normalize();
                Owner.Velocity *= speedLimit;
            }*/

            Owner.Acceleration = Vector2.Zero;

            Owner.Position = Owner.Position + (Owner.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds);

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void DebugDraw(SpriteBatch spriteBatch)
        {
            base.DebugDraw(spriteBatch);
        }
    }
}
