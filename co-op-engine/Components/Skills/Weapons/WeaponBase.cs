using co_op_engine.Sound;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Skills.Weapons
{
    public abstract class WeaponBase : SkillBase
    {
        private TimeSpan currentAttackTimer;

        public WeaponBase(SkillsComponent skillsComponent, GameObject owner)
            : base(skillsComponent, owner)
        { }

        override protected void UseSkill(int attackTimer = 0)
        {
            if (attackTimer == 0)
            {
                attackTimer = GetAnimationDuration(Constants.ACTOR_STATE_ATTACKING, Owner.FacingDirection);
            }
            currentAttackTimer = TimeSpan.FromMilliseconds(attackTimer);
            CurrentState = Constants.ACTOR_STATE_ATTACKING;
        }

        /// <summary>
        /// when the weapon hits something for the first time.
        /// </summary>
        protected abstract void WeaponHitSomething(GameObject thingHit);

        protected override void UpdateState(GameTime gameTime)
        {
            if (CurrentState == Constants.ACTOR_STATE_ATTACKING)
            {
                currentAttackTimer -= gameTime.ElapsedGameTime;
                if (currentAttackTimer <= TimeSpan.Zero)
                {
                    CurrentState = Constants.ACTOR_STATE_IDLE;
                    currentAttackTimer = TimeSpan.Zero;
                    ResetSkill(Constants.ACTOR_STATE_ATTACKING, Owner.FacingDirection);
                }
            }
        }

        // using this cause I'm making the assumption most weapons are going to behave like this, it could change later
        protected override void SkillHitObject(GameObject receiver)
        {
            if (HasntBeenHit(receiver) && Owner.Team != receiver.Team)
            {
                WeaponHitSomething(receiver);
            }
        }
    }
}
