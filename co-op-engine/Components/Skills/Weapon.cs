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

        override protected void PrimaryAttack(int attackTimer = 0)
        {
            if (attackTimer == 0)
            {
                attackTimer = GetAttackDuration();
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
                    ResetAttackAnimation();
                }
            }

            // switch to idle weapon animation of the player goes idle
            if (CurrentOwnerState == Constants.ACTOR_STATE_IDLE
                && CurrentStateProperties.CanInitiateIdleState)
            {
                CurrentState = Constants.ACTOR_STATE_IDLE;
            }

            // switch to walking weapon animation of the player started walking
            if (CurrentOwnerState == Constants.ACTOR_STATE_WALKING
                && CurrentStateProperties.CanInitiateWalkingState)
            {
                CurrentState = Constants.ACTOR_STATE_WALKING;
            }
        }


    }
}
