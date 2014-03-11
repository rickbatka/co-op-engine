using co_op_engine.Components.Weapons;
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


        public CombatBase(GameObject owner)
        {
            this.owner = owner;
        }

        virtual public void Update(GameTime gameTime)
        {
            UpdateWeaponTimers(gameTime);
        }

        virtual public void Draw(SpriteBatch spriteBatch) { }

        public void HandleHitByWeapon(WeaponBase hitByWeapon, int hitCooldownDurationMS)
        {
            // probably faster to use a dictionary of unique object ids for lookup once we get the IDs in place
            if (!weaponTimers.Any(wt => wt.Weapon == hitByWeapon))
            {
                owner.Health -= hitByWeapon.DamageRating;
                weaponTimers.Add(new WeaponTimer() { Weapon = hitByWeapon, Timer = new TimeSpan(0, 0, 0, 0, hitCooldownDurationMS) });
            }
        }

        private void UpdateWeaponTimers(GameTime gameTime)
        {
            var timersToRemove = new List<WeaponTimer>();

            foreach (var weaponTimer in weaponTimers)
            {
                weaponTimer.Timer -= gameTime.ElapsedGameTime;
                if (weaponTimer.Timer <= TimeSpan.Zero)
                {
                    timersToRemove.Add(weaponTimer);
                }
            }

            int cnt = timersToRemove.Count();
            for(int i = 0; i < cnt; i ++)
            {
                weaponTimers.Remove(timersToRemove[i]);
            }
        }
    }

    class WeaponTimer
    {
        public WeaponBase Weapon;
        public TimeSpan Timer;
    }
}
