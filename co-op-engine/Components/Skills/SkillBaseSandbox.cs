using co_op_engine.Components.Skills.StatusEffects;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Skills
{
    /// <summary>
    /// Abstract sandbox class for skills, the methods
    /// provided in this partial are used in the highest 
    /// implementation of skills as helper methods, skills
    /// at that level should not implement any interactions
    /// with other object but rather implement specific
    /// skill mechanical logic.
    /// </summary>
    public abstract partial class SkillBase
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

        protected void Knockback(GameObject to, int impulse)
        {
            var push = to.Position - Owner.Position;
            push.Normalize();
            push *= impulse;

            to.Velocity += push;
        }
    }
}
