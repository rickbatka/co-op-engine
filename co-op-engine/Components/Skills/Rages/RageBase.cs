using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Skills.Rages
{
    /// <summary>
    /// defines the rage behavior as a skill type, the player goes 
    /// into rage state until timer runs out, then returns to normal
    /// </summary>
    public abstract class RageBase : SkillBase
    {
        private TimeSpan currentRageTimer;
        public int RageCost { get; private set; }

        public RageBase(SkillsComponent skillsComponent, GameObject owner, int cost)
            : base(skillsComponent, owner)
        {
            RageCost = cost;
        }

        protected override void UpdateState(GameTime gameTime)
        {
            if (CurrentState == Constants.ACTOR_STATE_RAGING)
            {
                currentRageTimer -= gameTime.ElapsedGameTime;
                if (currentRageTimer <= TimeSpan.Zero)
                {
                    CurrentState = Constants.ACTOR_STATE_IDLE;
                    currentRageTimer = TimeSpan.Zero;
                    ResetSkill(Constants.ACTOR_STATE_RAGING, Owner.FacingDirection);
                }
            }
        }

        override protected void UseSkill(int attackTimer = 0)
        {
            if (attackTimer == 0)
            {
                attackTimer = GetAnimationDuration(Constants.ACTOR_STATE_RAGING, Owner.FacingDirection);
            }
            currentRageTimer = TimeSpan.FromMilliseconds(attackTimer);
            CurrentState = Constants.ACTOR_STATE_RAGING;
        }
    }
}
