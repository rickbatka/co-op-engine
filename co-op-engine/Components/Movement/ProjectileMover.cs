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

        //@TODO arg should take gameobject target, and should predict!!!
        public void HandleWasFired(object sender, FireProjectileEventArgs args)
        {
            Origin = Owner.Position;
            //change target to be edge of tower radius, so it always flies the same distance
            Target = GetTarget(Origin, args.TargetVector);
            IsTracking = true;
        }

        private Vector2 GetTarget(Vector2 origin, Vector2 target)
        {
            
            var difference = target - origin;
            var length = difference.Length();
            difference.Normalize();

            float newX = origin.X + difference.X * arrowDistance;
            float newY = origin.Y + difference.Y * arrowDistance;
            return new Vector2(newX, newY);
        }

        public override void Update(GameTime gameTime)
        {
            if(IsTracking && TimeShotFired == 0)
            {
                //mark the time we start tracking our path
                TimeShotFired = gameTime.TotalGameTime.TotalMilliseconds;
                var directionTowardTarget = Target - Owner.Position;
                directionTowardTarget.Normalize();

                Owner.RotationTowardFacingDirectionRadians = DrawingUtility.Vector2ToRadian(directionTowardTarget);

                
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
            }

            if (IsTracking)
            {
                Owner.Position = DrawingUtility.EaseInQuart(Origin, Target - Origin, ShotDuration, elapsedShotTime);
            }
        }
    }
}
