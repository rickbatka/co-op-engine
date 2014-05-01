using co_op_engine.Components.Particles;
using co_op_engine.Utility;
using co_op_engine.Utility.Camera;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Weapons.Effects
{
    public class BasicDamageEffect : WeaponEffectBase
    {
        int DamageRating;

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
            Receiver.CurrentState = Constants.ACTOR_STATE_BEING_HURT;

            GameTimerManager.Instance.SetTimer(
                time: Constants.BEING_HURT_EFFECT_TIME_MS,
                updateCallback: (t) =>
                {
                    Receiver.InputMovementVector = RotationAtTimeOfHit;
                },
                endCallback: (t) =>
                {
                    Receiver.InputMovementVector = Vector2.Zero;
                    Receiver.CurrentState = Constants.ACTOR_STATE_IDLE;
                }
            );
        }
    }
}
