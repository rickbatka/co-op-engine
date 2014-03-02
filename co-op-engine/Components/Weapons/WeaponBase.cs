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
        
        public int Width;
        public int Height;
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
            this.Width = renderer.animationSet.GetCurrentAnimationRectangle().CurrentDrawRectangle.Width;
            this.Height = renderer.animationSet.GetCurrentAnimationRectangle().CurrentDrawRectangle.Height;
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
            if (owner.currentActorState != ActorState.AttackingMelee)
            {
                AttackWithPrimaryMeleeWeapon();
            }
        }

        virtual public void AttackWithPrimaryMeleeWeapon()
        {
            currentAttackTimer = TimeSpan.FromMilliseconds(renderer.animationSet.GetAnimationDuration((int)ActorState.AttackingMelee, owner.FacingDirectionProp));
            owner.currentActorState = ActorState.AttackingMelee;
        }

        private void UpdateState(GameTime gameTime)
        {
            if(owner.currentActorState == ActorState.AttackingMelee)
            {
                currentAttackTimer -= gameTime.ElapsedGameTime;
                if (currentAttackTimer <= TimeSpan.Zero)
                {
                    owner.currentActorState = ActorState.Idle;
                    currentAttackTimer = TimeSpan.Zero;
                }
            }
        }

        public bool FullyRotatable { 
            get 
            {
                if (owner.currentActorState == ActorState.AttackingMelee)
                {
                    return true;
                }
                return false;
            } 
        }

        public Texture2D TextureProp { get { return texture; } set { texture = value; } }
        public Vector2 PositionProp { get { return owner.Position; } }
        public int WidthProp { get { return Width; } set { Width = value; } }
        public int HeightProp { get { return Height; } set { Height = value; } }
        public ActorState CurrentActorStateProp { get { return owner.currentActorState; } }
        public int FacingDirectionProp { get { return owner.FacingDirection; } }
        public Vector2 FacingDirectionRawProp { get { return owner.FacingDirectionRaw; } set { owner.FacingDirectionRaw = value; } }
        public float RotationTowardFacingDirectionRadiansProp { get { return owner.RotationTowardFacingDirectionRadians; } set { owner.RotationTowardFacingDirectionRadians = value; } }
    }
}
