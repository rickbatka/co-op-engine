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
    public abstract class WeaponBase : IRenderable
    {
        protected GameObject owner;
        protected RenderBase renderer;

        public int ID;
        protected TimeSpan currentAttackTimer;
        public int CurrentState { get; set; }
        protected WeaponState CurrentWeaponStateProperties { get { return WeaponStates.States[CurrentState]; } }

        public Frame CurrentFrame { get; set; }
        public Texture2D Texture { get; set; }
        public Vector2 Position { get { return owner.Position; } }
        public int FacingDirection { get { return owner.FacingDirection; } set { owner.FacingDirection = value; } }
        public Vector2 FacingDirectionRaw { get { return owner.FacingDirectionRaw; } set { owner.FacingDirectionRaw = value; } }
        public float RotationTowardFacingDirectionRadians { get { return owner.RotationTowardFacingDirectionRadians; } set { owner.RotationTowardFacingDirectionRadians = value; } }

        protected List<WeaponEffectBase> RealEffects = new List<WeaponEffectBase>();

        public WeaponBase(GameObject owner)
        {
            this.owner = owner;
            this.ID = MechanicSingleton.Instance.GetNextObjectCountValue();
        }

        public void SetRenderer(RenderBase renderer)
        {
            this.renderer = renderer;
        }

        virtual public void Update(GameTime gameTime)
        {
            UpdateState(gameTime);
            QueryForHits();
            renderer.Update(gameTime);
        }

        virtual public void Draw(SpriteBatch spriteBatch)
        {
            renderer.Draw(spriteBatch);
        }

        virtual public void DebugDraw(SpriteBatch spriteBatch)
        {
            renderer.DebugDraw(spriteBatch);
        }

        virtual public void TryInitiateAttack()
        {
            if (CurrentWeaponStateProperties.CanInitiatePrimaryAttack)
            {
                PrimaryAttack();
            }
        }

        virtual public void PrimaryAttack()
        {
            currentAttackTimer = TimeSpan.FromMilliseconds(renderer.animationSet.GetAnimationDuration(Constants.WEAPON_STATE_ATTACKING_PRIMARY, owner.FacingDirection));
            CurrentState = Constants.WEAPON_STATE_ATTACKING_PRIMARY;
        }

        private void QueryForHits()
        {
            if (CurrentWeaponStateProperties.IsAttacking)
            {
                var damageDots = renderer.CurrentAnimation.CurrentFrame.DamageDots;
                foreach (var damageDot in damageDots)
                {
                    var damageDotPositionVector = DrawingUtility.GetAbsolutePosition(this, damageDot.Location);
                    var colliders = owner.CurrentQuad.MasterQuery(DrawingUtility.VectorToPointRect(damageDotPositionVector));
                    foreach (var collider in colliders)
                    {
                        if(collider.ID != owner.ID)
                        {
                            collider.HandleHitByWeapon(this.ID, RealEffects, FacingDirectionRaw);
                        }
                    }
                }
            }
        }

        private void UpdateState(GameTime gameTime)
        {
            if (CurrentState == Constants.WEAPON_STATE_ATTACKING_PRIMARY)
            {
                currentAttackTimer -= gameTime.ElapsedGameTime;
                if (currentAttackTimer <= TimeSpan.Zero)
                {
                    CurrentState = Constants.WEAPON_STATE_IDLE;
                    currentAttackTimer = TimeSpan.Zero;
                    renderer.animationSet.GetAnimationFallbackToDefault(Constants.WEAPON_STATE_ATTACKING_PRIMARY, owner.FacingDirection).Reset();
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

        public void EquipRealEffect(WeaponEffectBase effect)
        {
            this.RealEffects.Add(effect);
        }
    }
}
