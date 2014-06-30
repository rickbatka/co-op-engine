using co_op_engine.Components.Skills.StatusEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Skills
{
    /// <summary>
    /// Abstract sandbox class for skills
    /// </summary>
    public abstract class SkillSandbox
    {
        /// <summary>
        /// Ready: Can use/ready to activate
        /// Using: 'casting' actively being used by player
        /// StillActive: Skill is still active, but player can stop casting/using, like a stormcloud was
        ///     summoned or something, we can deal with concurrent skills later if there's a need
        /// </summary>
        public enum SkillState { Ready, Using, StillActive };

        public abstract void Activate(GameObject owner);
        public SkillState CurrentSkillState;

        protected void DamageHealth(GameObject from, GameObject to, float amount)
        {
            to.Health.Value -= amount;
        }

        protected void AddSpeedBoost(GameObject to, TimeSpan duration, float amount)
        {
            var simpleBoost = new BoostSimpleStatusEffect(to, amount, duration);
            to.Combat.ApplyStatusEffect(simpleBoost);
        }

        protected void AddPoison(GameObject to, TimeSpan duration, float damage)
        {
            var poison = new SimplePoison(to, duration, damage);
            to.Combat.ApplyStatusEffect(poison);
        }
    }
}
