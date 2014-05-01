using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Weapons.Effects
{
    public class BasicHealEffect : WeaponEffectBase
    {
        int HealRating;

        public BasicHealEffect(int durationMS, int healRating)
            : base((int)EffectIdentifiers.REGEN_HEALTH, durationMS)
        {
            this.HealRating = healRating;
        }

        public override void Apply()
        {
            base.Apply();

            Receiver.Health += HealRating;
        }
    }
}
