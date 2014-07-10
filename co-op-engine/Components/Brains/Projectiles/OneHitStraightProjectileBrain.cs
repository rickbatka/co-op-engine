using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace co_op_engine.Components.Brains.Projectiles
{
    class OneHitStraightProjectileBrain : BrainBase
    {
        Vector2 Movement;
        TimeSpan deadTimer;

        public OneHitStraightProjectileBrain(GameObject owner, int life)
            : base(owner, false)
        {
            deadTimer = TimeSpan.FromMilliseconds(life);
        }

        public override void Update(GameTime gameTime)
        {
            deadTimer -= gameTime.ElapsedGameTime;
            if (deadTimer <= TimeSpan.Zero)
            {
                Owner.ShouldDelete = true;
            }
            Owner.InputMovementVector = Movement;
        }

        public void Shoot(Vector2 vector)
        {
            Movement = vector;
        }
    }
}
