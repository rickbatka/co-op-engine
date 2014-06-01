using co_op_engine.Collections;
using co_op_engine.Components.Rendering;
using co_op_engine.Components.Weapons.Effects;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Weapons
{
    public class Weapon : IRenderable
    {
        private GameObject owner;
        private RenderBase renderer;

        public int ID;
        private TimeSpan currentAttackTimer;
        public int CurrentState { get; set; }
        public virtual WeaponState CurrentWeaponStateProperties { get { return WeaponStates.States[CurrentState]; } }

        public bool Visible { get; set; }
        public bool Friendly { get { return owner.Friendly; } }
        public Frame CurrentFrame { get; set; }
        public Texture2D Texture { get; set; }
        public Vector2 Position { get { return owner.Position; } }
        public int FacingDirection { get { return owner.FacingDirection; } set { owner.FacingDirection = value; } }
        public float RotationTowardFacingDirectionRadians { get { return owner.RotationTowardFacingDirectionRadians; } set { owner.RotationTowardFacingDirectionRadians = value; } }
        public float Scale { get { return owner.Scale; } }

        public List<WeaponEffectBase> Effects = new List<WeaponEffectBase>();

        public Weapon(GameObject owner)
        {
            this.owner = owner;
            this.ID = MechanicSingleton.Instance.GetNextObjectCountValue();
            this.Visible = true;
        }

        public void SetRenderer(RenderBase renderer)
        {
            this.renderer = renderer;
        }

        public void Update(GameTime gameTime)
        {
            UpdateState(gameTime);
            QueryForHits();

            if(renderer != null)
            {
                renderer.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (renderer != null)
            {
                renderer.Draw(spriteBatch);
            }
        }

        public void DebugDraw(SpriteBatch spriteBatch)
        {
            if (renderer != null)
            {
                renderer.DebugDraw(spriteBatch);
            }
        }

        public void TryInitiateAttack()
        {
            if (CurrentWeaponStateProperties.CanInitiatePrimaryAttack)
            {
                PrimaryAttack();
            }
        }

        public void PrimaryAttack(int attackTimer = 0)
        {
            if(attackTimer == 0)
            {
                attackTimer = renderer.animationSet.GetAnimationDuration(Constants.WEAPON_STATE_ATTACKING_PRIMARY, owner.FacingDirection);
            }
            currentAttackTimer = TimeSpan.FromMilliseconds(attackTimer);
            CurrentState = Constants.WEAPON_STATE_ATTACKING_PRIMARY;
        }

        private void QueryForHits()
        {
            if (CurrentWeaponStateProperties.IsAttacking)
            {
                // damage dots come from the animated renderer, if there are any
                if (renderer != null)
                {
                    var damageDots = renderer.CurrentAnimation.CurrentFrame.DamageDots;
                    foreach (var damageDot in damageDots)
                    {
                        var damageDotPositionVector = DrawingUtility.GetAbsolutePosition(this, damageDot.Location);
                        var colliders = owner.CurrentQuad.MasterQuery(DrawingUtility.VectorToPointRect(damageDotPositionVector));
                        foreach (var collider in colliders)
                        {
                            if (collider.ID != owner.ID)
                            {
                                collider.HandleHitByWeapon(this);
                                FireUsedWeaponEvent(collider);
                            }
                        }
                    }
                }
            }
        }

        private void FireUsedWeaponEvent(GameObject receiver)
        {
            if(this.Friendly == receiver.Friendly)
            {
                owner.FireOnUsedWeaponEffectOnFriendly(this, null);
            }
            else
            {
                owner.FireOnDidAttackNonFriendly(this, null);
            }
        }

        protected virtual void UpdateState(GameTime gameTime)
        {
            if (CurrentState == Constants.WEAPON_STATE_ATTACKING_PRIMARY)
            {
                currentAttackTimer -= gameTime.ElapsedGameTime;
                if (currentAttackTimer <= TimeSpan.Zero)
                {
                    CurrentState = Constants.WEAPON_STATE_IDLE;
                    currentAttackTimer = TimeSpan.Zero;
                    if (renderer != null)
                    {
                        renderer.animationSet.GetAnimationFallbackToDefault(Constants.WEAPON_STATE_ATTACKING_PRIMARY, owner.FacingDirection).Reset();
                    }
                }
            }

            // switch to idle weapon animation of the player goes idle
            if (owner.CurrentState == Constants.ACTOR_STATE_IDLE
                && CurrentWeaponStateProperties.CanInitiateIdleState)
            {
                CurrentState = Constants.WEAPON_STATE_IDLE;
            }

            // switch to walking weapon animation of the player started walking
            if (owner.CurrentState == Constants.ACTOR_STATE_WALKING
                && CurrentWeaponStateProperties.CanInitiateWalkingState)
            {
                CurrentState = Constants.WEAPON_STATE_WALKING;
            }
        }

        public bool FullyRotatable { 
            get 
            {
                if (CurrentState == Constants.WEAPON_STATE_ATTACKING_PRIMARY)
                {
                    return true;
                }
                return false;
            } 
        }

        public void EquipEffect(WeaponEffectBase effect)
        {
            this.Effects.Add(effect);
        }
    }
}
