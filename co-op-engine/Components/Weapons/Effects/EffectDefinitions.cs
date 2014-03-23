using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Weapons.Effects
{
    public abstract class EffectDefinition
    {
        public int DurationMS;
        public abstract int UniqueIdentifier { get; }

        public EffectDefinition(int durationMS)
        {
            this.DurationMS = durationMS;
        }
    }

    public class BasicDamageEffectDefinition : EffectDefinition
    {
        public int DamageRating;
        public override int UniqueIdentifier { get { return (int)EffectIdentifiers.BASIC_DAMAGE; } }

        public BasicDamageEffectDefinition(int durationMS, int damageRating)
            :base(durationMS)
        {
            this.DamageRating = damageRating;
        }
    }

    public enum EffectIdentifiers
    { 
        BASIC_DAMAGE = 1
    }

    public static class WeaponEffectBuilder
    {
        public static WeaponEffectBase Build(GameObject receiver, int weaponId, EffectDefinition effectDef)
        {
            if (effectDef.UniqueIdentifier == (int)EffectIdentifiers.BASIC_DAMAGE)
            {
                return new BasicDamageEffect(receiver, weaponId, (BasicDamageEffectDefinition)effectDef);
            }
            throw new NotImplementedException("I don't know how to build that effect. Type: " + effectDef.UniqueIdentifier);
        }
    }
}
