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

            renderer.Update(gameTime);
            this.width = renderer.animationSet.GetCurrentAnimationRectangle().CurrentDrawRectangle.Width;
            this.height = renderer.animationSet.GetCurrentAnimationRectangle().CurrentDrawRectangle.Height;
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
            #warning check current state struct to see if we can initiate an attack
            if (owner.CurrentActorState != ActorState.AttackingMelee)
            {
                PrimaryAttack();
            }
        }

        virtual public void PrimaryAttack()
        {
            currentAttackTimer = TimeSpan.FromMilliseconds(renderer.animationSet.GetAnimationDuration((int)ActorState.AttackingMelee, owner.FacingDirection));
            owner.CurrentActorState = ActorState.AttackingMelee;
        }

        private void UpdateState(GameTime gameTime)
        {
            if(owner.CurrentActorState == ActorState.AttackingMelee)
            {
                currentAttackTimer -= gameTime.ElapsedGameTime;
                if (currentAttackTimer <= TimeSpan.Zero)
                {
                    owner.CurrentActorState = ActorState.Idle;
                    currentAttackTimer = TimeSpan.Zero;
                }
            }
        }

        public bool FullyRotatable { 
            get 
            {
                if (owner.CurrentActorState == ActorState.AttackingMelee)
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
        public ActorState CurrentActorState { get { return owner.CurrentActorState; } set { owner.CurrentActorState = value; } }
        public int FacingDirection { get { return owner.FacingDirection; } set { owner.FacingDirection = value; } }
        public Vector2 FacingDirectionRaw { get { return owner.FacingDirectionRaw; } set { owner.FacingDirectionRaw = value; } }
        public float RotationTowardFacingDirectionRadians { get { return owner.RotationTowardFacingDirectionRadians; } set { owner.RotationTowardFacingDirectionRadians = value; } }
    }
}
