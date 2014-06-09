using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Skills.Weapons
{
    public class AlwaysAttackingWeapon : Weapon
    {
        public AlwaysAttackingWeapon(SkillsComponent skillsComponent, GameObject owner)
        :base(skillsComponent, owner)
        { }

        protected override void UpdateState(GameTime gameTime)
        {
            if(CurrentState != Constants.ACTOR_STATE_ATTACKING)
            {
                CurrentState = Constants.ACTOR_STATE_ATTACKING;
            }
        }
    }
}
