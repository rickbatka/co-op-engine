using co_op_engine.Components.Particles;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Movement
{
    public class ProjectileMover : MoverBase
    {
        private Vector2 Origin;
        private Vector2 Target;
        private double TimeShotFired = 0;
        private bool IsTracking = false;
        private float arrowDistance = 500f;

        //this is tuned to fly really fast at short range and still be able to hit at around ~700 pixels away
        private float ShotDuration = 550f;

        public ProjectileMover(GameObject owner)
            : base(owner)
        {
            owner.OnWasFiredAtFixedPoint += HandleWasFired;
        }

        public void HandleWasFired(object sender, FireProjectileEventArgs args)
        {
            Origin = Owner.Position;
            Target = GetTarget(Origin, args.TargetObject);
            IsTracking = true;
        }

        private Vector2 GetTarget(Vector2 origin, GameObject target)
        {
            // lame interpolation to try to lead the target and actually hit them once in a while...
            var newTarget = target.Position + (target.Velocity * ((ShotDuration / 2) / 1000f));
            var difference = newTarget - origin;
            difference.Normalize();

            // rotate arrow toward target
            Owner.RotationTowardFacingDirectionRadians = DrawingUtility.Vector2ToRadian(difference);
            
            //change target to be edge of tower radius, so it always flies the same distance
            float newX = origin.X + difference.X * arrowDistance;
            float newY = origin.Y + difference.Y * arrowDistance;
            newTarget = new Vector2(newX, newY);
            
            return newTarget;
        }

        public override void Update(GameTime gameTime)
        {
            if(IsTracking && TimeShotFired == 0)
            {
                //mark the time we start tracking our path
                TimeShotFired = gameTime.TotalGameTime.TotalMilliseconds;
            }

            if(IsTracking)
            {
                FlyTowardTarget(gameTime);
            }

            base.Update(gameTime);
        }

        private void FlyTowardTarget(GameTime gameTime)
        {
            var elapsedShotTime = (float)(gameTime.TotalGameTime.TotalMilliseconds - TimeShotFired);
            if(elapsedShotTime >= ShotDuration)
            {
                //todo puff of smoke if it misses, then disappear
                IsTracking = false;
                Owner.ShouldDelete = true;
                ParticleEngine.Instance.AddEmitter(
                    new DustFastEmitter(Owner)
                );
            }

            if (IsTracking)
            {
                Owner.Position = DrawingUtility.EaseInQuart(Origin, Target - Origin, ShotDuration, elapsedShotTime);
            }
        }
    }
}
