using co_op_engine.Components.Particles;
using co_op_engine.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Weapons.Effects
{
    public class BasicDamageEffect : WeaponEffectBase
    {
        int DamageRating;

        public BasicDamageEffect(GameObject receiver, int weaponId, BasicDamageEffectDefinition def)
            :base(receiver, weaponId, def.DurationMS)
        {
            this.DamageRating = def.DamageRating;
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
