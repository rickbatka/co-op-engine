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
        protected Texture2D texture;
        
        protected int width;
        protected int height;
        protected TimeSpan currentAttackTimer;

        public int DamageRating = 25;
        public int HitCooldownDurationMS = 300;

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
            this.width = renderer.animationSet.GetAnimationFallbackToDefault(renderer.animationSet.currentState, renderer.animationSet.currentFacingDirection).CurrentDrawRectangle.Width;
            this.height = renderer.animationSet.GetAnimationFallbackToDefault(renderer.animationSet.currentState, renderer.animationSet.currentFacingDirection).CurrentDrawRectangle.Height;
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
            if (owner.CurrentStateProperties.CanInitiatePrimaryAttackState)
            {
                PrimaryAttack();
            }
        }

        virtual public void PrimaryAttack()
        {
            currentAttackTimer = TimeSpan.FromMilliseconds(renderer.animationSet.GetAnimationDuration(Constants.STATE_ATTACKING_MELEE, owner.FacingDirection));
            owner.CurrentActorState = Constants.STATE_ATTACKING_MELEE;
        }

        private void DoDamage()
        {
            if (owner.CurrentStateProperties.IsAttacking)
            {
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
                        if(collider != owner)
                        {
                            collider.HandleHitByWeapon(this, HitCooldownDurationMS);
                        }
                    }
                }
            }
        }

        private void UpdateState(GameTime gameTime)
        {
            if (owner.CurrentActorState == Constants.STATE_ATTACKING_MELEE)
            {
                currentAttackTimer -= gameTime.ElapsedGameTime;
                if (currentAttackTimer <= TimeSpan.Zero)
                {
                    owner.CurrentActorState = Constants.STATE_IDLE;
                    currentAttackTimer = TimeSpan.Zero;
                    renderer.animationSet.GetAnimationFallbackToDefault(Constants.STATE_ATTACKING_MELEE, owner.FacingDirection).Reset();
                }
            }
        }

        public bool FullyRotatable { 
            get 
            {
                if (owner.CurrentActorState == Constants.STATE_ATTACKING_MELEE)
                {
                    return true;
                }
                return false;
            } 
        }

        public Texture2D Texture { get { return texture; } set { texture = value; } }
        public Vector2 Position { get { return owner.Position; } }
        public int Width { get { return width; } set { width = value; } }
        public int Height { get { return height; } set { height = value; } }
        public int CurrentActorState { get { return owner.CurrentActorState; } set { owner.CurrentActorState = value; } }
        public int FacingDirection { get { return owner.FacingDirection; } set { owner.FacingDirection = value; } }
        public Vector2 FacingDirectionRaw { get { return owner.FacingDirectionRaw; } set { owner.FacingDirectionRaw = value; } }
        public float RotationTowardFacingDirectionRadians { get { return owner.RotationTowardFacingDirectionRadians; } set { owner.RotationTowardFacingDirectionRadians = value; } }
    }
}
