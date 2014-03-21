using co_op_engine.Collections;
using co_op_engine.Components.Rendering;
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
        protected AnimatedRenderer renderer;

        protected TimeSpan currentAttackTimer;
        public int DamageRating = 25;
        public int HitCooldownDurationMS = 300;
        public int CurrentState { get; set; }
        protected WeaponState CurrentWeaponStateProperties { get { return WeaponStates.States[CurrentState]; } }

        public Texture2D Texture { get; set; }
        public Vector2 Position { get { return owner.Position; } }
        public int Width { get; set; }
        public int Height { get; set; }
        public int FacingDirection { get { return owner.FacingDirection; } set { owner.FacingDirection = value; } }
        public Vector2 FacingDirectionRaw { get { return owner.FacingDirectionRaw; } set { owner.FacingDirectionRaw = value; } }
        public float RotationTowardFacingDirectionRadians { get { return owner.RotationTowardFacingDirectionRadians; } set { owner.RotationTowardFacingDirectionRadians = value; } }



        public WeaponBase(GameObject owner)
        {
            this.owner = owner;
        }

        public void SetRenderer(AnimatedRenderer renderer)
        {
            this.renderer = renderer;
        }

        virtual public void Update(GameTime gameTime)
        {
            UpdateState(gameTime);
            DoDamage();
            renderer.Update(gameTime);

#warning rick
            this.Width = renderer.animationSet.GetAnimationFallbackToDefault(renderer.animationSet.currentState, renderer.animationSet.currentFacingDirection).CurrentDrawRectangle.Width;
            this.Height = renderer.animationSet.GetAnimationFallbackToDefault(renderer.animationSet.currentState, renderer.animationSet.currentFacingDirection).CurrentDrawRectangle.Height;
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

        private void DoDamage()
        {
            if (CurrentWeaponStateProperties.IsAttacking)
            {
#warning rick
                var damageDots = renderer.animationSet.GetAnimationFallbackToDefault(renderer.animationSet.currentState, renderer.animationSet.currentFacingDirection).CurrentFrame.DamageDots;
                foreach (var damageDot in damageDots)
                {
                    var colliders = owner.CurrentQuad.MasterQuery
                    (
                        DrawingUtility.VectorToPointRect
                        (
                            DrawingUtility.GetAbsolutePosition(owner, damageDot.Location)
                        )
                    );
                    foreach (var collider in colliders)
                    {
                        if(collider.ID != owner.ID)
                        {
                            collider.HandleHitByWeapon(this, HitCooldownDurationMS);
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
    }
}
