using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Skills
{
    public class Weapon : Skill
    {
        private TimeSpan currentAttackTimer;

        public Weapon(SkillsComponent skillsComponent, GameObject owner)
            : base(skillsComponent, owner)
        { }

        override public bool TryInitiateSkill(int attackTimer = 0)
        {
            if (CurrentStateProperties.CanInitiateSkills)
            {
                UseSkill(attackTimer);
                return true;
            }
            return false;
        }

        override protected void UseSkill(int attackTimer = 0)
        {
            if (attackTimer == 0)
            {
                attackTimer = GetAnimationDuration(Constants.ACTOR_STATE_ATTACKING, Owner.FacingDirection);
            }
            currentAttackTimer = TimeSpan.FromMilliseconds(attackTimer);
            CurrentState = Constants.ACTOR_STATE_ATTACKING;
        }   

        protected override void UpdateState(GameTime gameTime)
        {
            if (CurrentState == Constants.ACTOR_STATE_ATTACKING)
            {
                currentAttackTimer -= gameTime.ElapsedGameTime;
                if (currentAttackTimer <= TimeSpan.Zero)
                {
                    CurrentState = Constants.ACTOR_STATE_IDLE;
                    currentAttackTimer = TimeSpan.Zero;
                    ResetAnimation(Constants.ACTOR_STATE_ATTACKING, Owner.FacingDirection);
                }
            }
        }
    }
}
