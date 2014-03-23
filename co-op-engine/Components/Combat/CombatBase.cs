using co_op_engine.Components.Weapons;
using co_op_engine.Components.Weapons.Effects;
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

        protected TimeSpan RecoveryTimer;
        List<WeaponTimer> weaponTimers = new List<WeaponTimer>();
        Dictionary<string, WeaponEffectBase> effectsByWeapon = new Dictionary<string, WeaponEffectBase>();


        public CombatBase(GameObject owner)
        {
            this.owner = owner;
        }

        virtual public void Update(GameTime gameTime)
        {
            RemoveFinishedWeaponEffects();
            UpdateCurrentWeaponEffects(gameTime);
        }

        virtual public void Draw(SpriteBatch spriteBatch) { }

        public void HandleHitByWeapon(int weaponId, List<EffectDefinition> effects)
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

        private string GetHash(int weaponId, EffectDefinition effect)
        {
            return "" + weaponId + "_" + effect.UniqueIdentifier;
        }
    }

    class WeaponTimer
    {
        public WeaponBase Weapon;
        public TimeSpan Timer;
    }
}
