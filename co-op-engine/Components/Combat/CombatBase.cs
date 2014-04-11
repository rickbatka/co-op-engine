using co_op_engine.Components.Particles;
using co_op_engine.Components.Weapons;
using co_op_engine.Components.Weapons.Effects;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Combat
{
    public class CombatBase
    {
        protected GameObject owner;
        Dictionary<string, WeaponEffectBase> effectsByWeapon = new Dictionary<string, WeaponEffectBase>();

        private Vector2 beingHurtHitRotation;
        private TimeSpan beingHurtTimer;
        private TimeSpan dyingAnimationTimer;

        public CombatBase(GameObject owner)
        {
            this.owner = owner;
        }

        virtual public void Update(GameTime gameTime)
        {
            RemoveFinishedWeaponEffects();
            UpdateCurrentWeaponEffects(gameTime);

            MaybeStartDyingFromWounds();
        }

        virtual public void Draw(SpriteBatch spriteBatch) { }

        public void HandleHitByWeapon(int weaponId, List<EffectDefinition> effects, Vector2 hitRotation)
        {
            foreach(var effect in effects)
            {
                string hash = GetHash(weaponId, effect);
                if (owner.CurrentStateProperties.IsVulnerable 
                    && !effectsByWeapon.ContainsKey(hash))
                {
                    var newEffect = WeaponEffectBuilder.Build(receiver: owner, weaponId: weaponId, effectDef: effect);
                    effectsByWeapon.Add(hash, newEffect);

                    // Apply the status effect
                    newEffect.Apply();

                    KnockBack(hitRotation);

                    ParticleEngine.Instance.AddEmitter(
                        //new BloodHitEmitter(owner, hitRotation)
                        new FireEmitter(owner)
                    );
                }
            }
        }

        private void KnockBack(Vector2 hitRotation)
        {
            owner.CurrentState = Constants.ACTOR_STATE_BEING_HURT;
            beingHurtHitRotation = hitRotation;
            beingHurtTimer = TimeSpan.FromMilliseconds(Constants.BEING_HURT_EFFECT_TIME_MS);
            GameTimerManager.Instance.SetTimer(
                time: Constants.BEING_HURT_EFFECT_TIME_MS,
                updateCallback: (t) =>
                {
                    if (!owner.UnShovable)
                    {
                        owner.InputMovementVector = beingHurtHitRotation;
                    }
                },
                endCallback: (t) =>
                {
                    if (!owner.UnShovable)
                    {
                        owner.InputMovementVector = Vector2.Zero;
                    }
                    owner.CurrentState = Constants.ACTOR_STATE_IDLE;
                }
            );
        }

        private void UpdateCurrentWeaponEffects(GameTime gameTime)
        {
            foreach (var effect in effectsByWeapon.Values)
            {
                // Update the status effect
                effect.Update(gameTime);
            }
        }

        private void RemoveFinishedWeaponEffects()
        {
            var effectsToRemove = new List<string>();
            foreach(var key in effectsByWeapon.Keys)
            {
                if(effectsByWeapon[key].IsFinished)
                {
                    effectsToRemove.Add(key);
                }
            }

            int cnt = effectsToRemove.Count();
            for (int i = 0; i < cnt; i++)
            {
                // Clear the status effect
                effectsByWeapon[effectsToRemove[i]].Clear();
                effectsByWeapon.Remove(effectsToRemove[i]);
            }
        }

        private void MaybeStartDyingFromWounds()
        {
            if (owner.Health <= 0)
            {
                owner.Health = 0;
                if (owner.CurrentStateProperties.CanStartDying)
                {
                    owner.CurrentState = Constants.ACTOR_STATE_DYING;
                    dyingAnimationTimer = TimeSpan.FromMilliseconds(owner.Renderer.animationSet.GetAnimationDuration(Constants.ACTOR_STATE_DYING, owner.FacingDirection));

                    // animate dying for x milliseconds, then kill
                    GameTimerManager.Instance.SetTimer(
                        time: owner.Renderer.animationSet.GetAnimationDuration(Constants.ACTOR_STATE_DYING, owner.FacingDirection),
                        updateCallback: (t) => { },
                        endCallback: (t) => { owner.CurrentState = Constants.ACTOR_STATE_DEAD; }
                    );
                }
            }
        }

        private string GetHash(int weaponId, EffectDefinition effect)
        {
            return "" + weaponId + "_" + effect.UniqueIdentifier;
        }

        internal void ReceiveCommand(Networking.Commands.GameObjectCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
