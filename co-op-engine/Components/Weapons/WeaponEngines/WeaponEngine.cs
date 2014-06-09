using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Weapons.WeaponEngines
{
    /*
    public class WeaponEngine
    {
        protected Weapon Weapon;

        private TimeSpan currentAttackTimer;

        public WeaponEngine(Weapon weapon)
        {
            this.Weapon = weapon;
        }

        virtual public void Draw(SpriteBatch spriteBatch) { }

        virtual public void DebugDraw(SpriteBatch spriteBatch) { }

        virtual public void Update(GameTime gameTime) 
        {
            QueryForHits();
            UpdateState(gameTime);
        }

        public void PrimaryAttack(int attackTimer = 0)
        {
            if (attackTimer == 0)
            {
                attackTimer = Weapon.GetAttackDuration();
            }
            currentAttackTimer = TimeSpan.FromMilliseconds(attackTimer);
            Weapon.CurrentState = Constants.WEAPON_STATE_ATTACKING_PRIMARY;
        }

        private void QueryForHits()
        {
            if (Weapon.CurrentWeaponStateProperties.IsAttacking
                && Weapon.CurrentFrame.DamageDots != null)
            {
                
                var damageDots = Weapon.CurrentFrame.DamageDots;
                foreach (var damageDot in damageDots)
                {
                    var damageDotPositionVector = DrawingUtility.GetAbsolutePosition(Weapon, damageDot.Location);
                    var colliders = Weapon.CurrentQuad.MasterQuery(DrawingUtility.VectorToPointRect(damageDotPositionVector));
                    foreach (var collider in colliders)
                    {
                        if (collider.ID != Weapon.OwnerId)
                        {
                            collider.HandleHitByWeapon(Weapon);
                            Weapon.FireUsedWeaponEvent(collider);
                        }
                    }
                }
                
            }
        }

        protected virtual void UpdateState(GameTime gameTime)
        {
            if (Weapon.CurrentState == Constants.WEAPON_STATE_ATTACKING_PRIMARY)
            {
                currentAttackTimer -= gameTime.ElapsedGameTime;
                if (currentAttackTimer <= TimeSpan.Zero)
                {
                    Weapon.CurrentState = Constants.WEAPON_STATE_IDLE;
                    currentAttackTimer = TimeSpan.Zero;
                    Weapon.ResetAttackAnimation();
                }
            }

            // switch to idle weapon animation of the player goes idle
            if (Weapon.CurrentOwnerState == Constants.ACTOR_STATE_IDLE
                && Weapon.CurrentWeaponStateProperties.CanInitiateIdleState)
            {
                Weapon.CurrentState = Constants.WEAPON_STATE_IDLE;
            }

            // switch to walking weapon animation of the player started walking
            if (Weapon.CurrentOwnerState == Constants.ACTOR_STATE_WALKING
                && Weapon.CurrentWeaponStateProperties.CanInitiateWalkingState)
            {
                Weapon.CurrentState = Constants.WEAPON_STATE_WALKING;
            }
        }
    }
     * */
}
