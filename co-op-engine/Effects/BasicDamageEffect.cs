using co_op_engine.Components.Particles;
using co_op_engine.Components.Particles.Emitters;
using co_op_engine.Utility;
using co_op_engine.Utility.Camera;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Effects
{
    public class BasicDamageEffect : StatusEffect
    {
        int DamageRating;
        private const int KNOCKBACK_FORCE = 3000;

        public BasicDamageEffect(int durationMS, int damageRating)
            :base((int)EffectIdentifiers.BASIC_DAMAGE, durationMS)
        {
            this.DamageRating = damageRating;
        }

        public override void Apply()
        {
            base.Apply();

            Receiver.Health -= DamageRating;
            KnockBack();
            ParticleEngine.Instance.AddEmitter(
                 new BloodHitEmitter(Receiver, RotationAtTimeOfHit)
            );

            //@TODO camera should subscribe to OnDeath and do this in response
            if (Receiver.Health <= 0)
            {
                Camera.Instance.Shake();
                ParticleEngine.Instance.AddEmitter(
                    new FireEmitter(Receiver)
                );
            }
        }

        private void KnockBack()
        {
            Receiver.Velocity += KNOCKBACK_FORCE * RotationAtTimeOfHit;
        }
    }
}
