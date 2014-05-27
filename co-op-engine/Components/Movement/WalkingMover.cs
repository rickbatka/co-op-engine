using co_op_engine.Utility;
using co_op_engine.Utility.Camera;
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
        float _force = 1;
        float _mu = 0.5f;

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

        /*
         * current physics triad from: Mu(Vm + F) = F
         * Mu = F / (Vm + F)
         * V = (F - FMu) / Mu
         * F = -MuVm / (Mu - 1)
         */

        public override void Update(GameTime gameTime)
        {
            //assuming gametime factor of 60fps
            float frameAdj = (float)(gameTime.ElapsedGameTime.TotalSeconds * 60);
            if (Owner.InputMovementVector.Length() > 0) Owner.InputMovementVector.Normalize();
            Vector2 force = Owner.InputMovementVector * _force;

            //primary equation, applies friction scaling with velocity to give a horizontal asymtote
            Owner.Velocity = -_mu * (Owner.Velocity + force) + (Owner.Velocity + force);

            //add velocity to position to simulate movement
            Owner.Position += Owner.Velocity * frameAdj/60;

            base.Update(gameTime);
        }

        public void DefineNoMu(float maxVelocity, float force)
        {
            _mu = force / (maxVelocity + force);
            _force = force;
        }

        public void DefineNoF(float maxVelocity, float mu)
        {
            _force = (-mu * maxVelocity) / (mu - 1);
            _mu = mu;
        }

        public void Define(float force, float friction)
        {
            _force = force;
            _mu = friction;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void DebugDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(
                    spriteFont: AssetRepository.Instance.Arial,
                    text: "Velocity: " + Owner.Velocity.Length().ToString(),
                    position: new Vector2(Camera.Instance.ViewBoundsRectangle.Left + 25, Camera.Instance.ViewBoundsRectangle.Top + 45),
                    color: Color.White,
                    rotation: 0f,
                    origin: Vector2.Zero,
                    scale: 1f,
                    effects: SpriteEffects.None,
                    depth: 1f
                );
            base.DebugDraw(spriteBatch);
        }
    }
}
