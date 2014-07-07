using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Skills.Spells
{
    /// <summary>
    /// Purpose: 2nd level abstract base class to set 
    ///     the functionality of all spell skills.
    /// 
    /// This should not be modified without considering
    ///     all spells in codebase.
    /// </summary>
    public abstract class SpellBase : SkillBase
    {
        protected TimeSpan CooldownTimer;
        private int CooldownMilli;
        public bool IsReady { get { return CooldownTimer <= TimeSpan.Zero; } }

        public SpellBase(SkillsComponent skillsComponent, GameObject owner, int cooldownMilli)
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

        protected override void SkillHitObject(GameObject receiver)
        {
            //do nothing, assuming spells will mostly shoot projectiles or something that doesn't directly interact with world
        }
    }
}
