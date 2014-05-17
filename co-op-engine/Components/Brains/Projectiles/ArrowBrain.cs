using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Brains.Projectiles
{
    public class ArrowBrain : BrainBase
    {
        private GameObject Target;
        private float speedModifier = 2f;

        public ArrowBrain(GameObject owner, GameObject target)
            :base(owner, false)
        {
            Target = target;
            Owner.OnDidAttackNonFriendly += HandleArrowHitTarget;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            var directionTowardTarget = Target.Position - Owner.Position;
            directionTowardTarget.Normalize();

            Owner.RotationTowardFacingDirectionRadians = DrawingUtility.Vector2ToRadian(directionTowardTarget);

            directionTowardTarget *= speedModifier;

            Owner.InputMovementVector = directionTowardTarget;
        }

        private void HandleArrowHitTarget(object sender, EventArgs e)
        {
            Owner.ShouldDelete = true;
        }
    }
}
