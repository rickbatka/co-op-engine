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
            UpdateDying(gameTime);
        }

        virtual public void Draw(SpriteBatch spriteBatch) { }

        public void HandleHitByWeapon(int weaponId, List<EffectDefinition> effects, Vector2 hitRotation)
        {
            foreach(var effect in effects)
            {
                string hash = GetHash(weaponId, effect);
                if (!effectsByWeapon.ContainsKey(hash))
                {
                    var newEffect = WeaponEffectBuilder.Build(receiver: owner, weaponId: weaponId, effectDef: effect);
                    effectsByWeapon.Add(hash, newEffect);

                    // Apply the status effect
                    newEffect.Apply();

                    ParticleEngine.Instance.AddEmitter(
                        //new BloodHitEmitter(owner, hitRotation)
                        new FireEmitter(owner)
                    );
                }
            }
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
                    dyingAnimationTimer = TimeSpan.FromMilliseconds(owner.Renderer.animationSet.GetAnimationDuration(Constants.WEAPON_STATE_ATTACKING_PRIMARY, owner.FacingDirection)); 
                }
            }
        }

        private void UpdateDying(GameTime gameTime)
        {
            if (owner.CurrentStateProperties.CanFinishDying && dyingAnimationTimer != null)
            {
                dyingAnimationTimer -= gameTime.ElapsedGameTime;
                if (dyingAnimationTimer <= TimeSpan.Zero)
                {
                    owner.CurrentState = Constants.ACTOR_STATE_DEAD;
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
