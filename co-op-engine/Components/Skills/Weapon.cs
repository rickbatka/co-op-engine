using co_op_engine.Sound;
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

        override public void Activate(int attackTimer = 0)
        {
            UseSkill(attackTimer);
        }

        override protected void UseSkill(int attackTimer = 0)
        {
            if (attackTimer == 0)
            {
                attackTimer = GetAnimationDuration(Constants.ACTOR_STATE_ATTACKING, Owner.FacingDirection);
            }
            currentAttackTimer = TimeSpan.FromMilliseconds(attackTimer);
            CurrentState = Constants.ACTOR_STATE_ATTACKING;
            SoundManager.PlaySoundEffect(AssetRepository.Instance.SwordSwoosh1);
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
                    ResetSkill(Constants.ACTOR_STATE_ATTACKING, Owner.FacingDirection);
                }
            }
        }

        //HACK this is hack until I get a non base class for sword... need this to build currently
        protected override void SkillHitObject(GameObject receiver)
        {
            if (HasntBeenHit(receiver))
            {
                DamageHealth(this.Owner, receiver, 10);
                Knockback(receiver, 5000);
            }
        }
    }
}
