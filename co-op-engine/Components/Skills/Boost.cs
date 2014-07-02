using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Skills
{
    /// <summary>
    /// more of a placeholder, boosts aren't really confined to behavior
    /// </summary>
    public abstract class Boost : Skill
    {
        protected TimeSpan CooldownTimer;
        private int CooldownMilli;
        public bool IsReady { get { return CooldownTimer <= TimeSpan.Zero; } }

        public Boost(SkillsComponent skillsComponent, GameObject owner, int cooldownMilli)
            :base(skillsComponent, owner)
        {
            CooldownTimer = TimeSpan.Zero;
            CooldownMilli = cooldownMilli;
        }

        protected override void UpdateState(GameTime gameTime)
        {
            CooldownTimer -= gameTime.ElapsedGameTime;
        }

        protected override void UseSkill(int attackTimer = 0)
        {
            CooldownTimer = TimeSpan.FromMilliseconds(CooldownMilli);
        }
    }
}
