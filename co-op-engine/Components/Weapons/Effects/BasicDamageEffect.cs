using co_op_engine.Components.Particles;
using co_op_engine.Utility;
using co_op_engine.Utility.Camera;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Weapons.Effects
{
    public class BasicDamageEffect : WeaponEffectBase
    {
        int DamageRating;

        public BasicDamageEffect(int weaponEffectId, int durationMS, int damageRating)
            :base(weaponEffectId, durationMS)
        {
            this.DamageRating = damageRating;
        }

        public override object Clone()
        {
            return this.MemberwiseClone();
        }

        public override void Apply()
        {
            base.Apply();

            Receiver.Health -= DamageRating;

            if(Receiver.Health <= 0)
            {
                Camera.Instance.Shake();
            }
        }
    }
}
