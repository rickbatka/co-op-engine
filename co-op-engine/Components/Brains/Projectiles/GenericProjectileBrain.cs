using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Brains.Projectiles
{
    public class GenericProjectileBrain : BrainBase
    {
        public GenericProjectileBrain(GameObject owner)
            :base(owner, false)
        {
            Owner.OnDidAttackNonFriendly += HandleArrowHitTarget;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        private void HandleArrowHitTarget(object sender, EventArgs e)
        {
            Owner.ShouldDelete = true;
        }
    }
}
