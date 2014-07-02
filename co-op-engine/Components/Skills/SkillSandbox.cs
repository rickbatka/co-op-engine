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
    public abstract partial class Skill
    {
        protected void DamageHealth(GameObject from, GameObject to, float amount)
        {
            to.Health.Value -= amount;
        }

        protected void ApplySpeedModifier(GameObject to, int durationMilli, float amount)
        {
            var simpleBoost = new BoostSimpleStatusEffect(to, amount, durationMilli);
            to.Combat.ApplyStatusEffect(simpleBoost);
        }

        protected void AddDamageOverTime(GameObject to, int durationMilli, int tickIntervalMilli, float damagePerTick)//here's where we could add types later
        {
            var poison = new SimplePoison(to, durationMilli, tickIntervalMilli, damagePerTick);
            to.Combat.ApplyStatusEffect(poison);
        }
    }
}
