using co_op_engine.Collections;
using co_op_engine.Components.Rendering;
using co_op_engine.Effects;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Skills
{
    public abstract class Skill : IRenderable
    {
        protected GameObject Owner;
        protected SkillsComponent SkillsComponent;
        private RenderBase Renderer;
        protected SpacialBase CurrentQuad { get { return Owner.CurrentQuad; } }

        public int ID;
        public int OwnerId;

        public int CurrentState { get { return Owner.CurrentState; } set { Owner.CurrentState = value; } }
        public ActorState CurrentStateProperties { get { return ActorStates.States[CurrentState]; } }

        public bool Visible { get; set; }
        public bool Friendly { get { return Owner.Friendly; } }
        public Frame CurrentFrame { get; set; }
        public Texture2D Texture { get; set; }
        public Vector2 Position { get { return new Vector2(Owner.Position.X, Owner.Position.Y - 1);  }}//Owner.Position; } }
        public int FacingDirection { get { return Owner.FacingDirection; } }
        public float RotationTowardFacingDirectionRadians { get { return Owner.RotationTowardFacingDirectionRadians; } }
        public float Scale { get; set; }


        public List<StatusEffect> Effects = new List<StatusEffect>();

        public Skill(SkillsComponent skillsComponent, GameObject owner)
        {
            this.SkillsComponent = skillsComponent;
            this.Owner = owner;
            this.ID = MechanicSingleton.Instance.GetNextObjectCountValue();
            this.OwnerId = owner.ID;
            this.Visible = true;
            this.Scale = owner.Scale;
        }

        public void SetRenderer(RenderBase renderer)
        {
            this.Renderer = renderer;
        }

        protected abstract void UseSkill(int attackTimer = 0);
        protected abstract void UpdateState(GameTime gameTime);
        public abstract bool TryInitiateSkill(int attackTimer = 0);
        
        public virtual void Update(GameTime gameTime)
        {
            UpdateState(gameTime);
            QueryForHits();

            if(Renderer != null)
            {
                Renderer.Update(gameTime);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (Renderer != null)
            {
                Renderer.Draw(spriteBatch);
            }
        }

        public virtual void DebugDraw(SpriteBatch spriteBatch)
        {
            if (Renderer != null)
            {
                Renderer.DebugDraw(spriteBatch);
            }
        }

        public int GetAnimationDuration(int state, int facingDirection)
        {
            if (Renderer != null)
            {
                return Renderer.animationSet.GetAnimationDuration(state, facingDirection);
            }

            return 0;
        }

        public void FireUsedWeaponEvent(GameObject receiver)
        {
            if(this.Friendly == receiver.Friendly)
            {
                Owner.FireOnUsedWeaponEffectOnFriendly(this, null);
            }
            else
            {
                Owner.FireOnDidAttackNonFriendly(this, null);
            }
        }

        public bool FullyRotatable { 
            get 
            {
                if (CurrentState == Constants.ACTOR_STATE_ATTACKING)
                {
                    return true;
                }
                return false;
            } 
        }

        public void ResetAnimation(int state, int facingDirection)
        {
            if (Renderer != null)
            {
                Renderer.animationSet.GetAnimationFallbackToDefault(state, facingDirection).Reset();
            }
        }

        public void EquipEffect(StatusEffect effect)
        {
            this.Effects.Add(effect);
        }

        protected virtual void QueryForHits()
        {
            if (CurrentStateProperties.IsAttacking
                && CurrentFrame.DamageDots != null)
            {

                var damageDots = CurrentFrame.DamageDots;
                foreach (var damageDot in damageDots)
                {
                    var damageDotPositionVector = DrawingUtility.GetAbsolutePosition(this, damageDot.Location);
                    var colliders = CurrentQuad.MasterQuery(DrawingUtility.VectorToPointRect(damageDotPositionVector));
                    foreach (var collider in colliders)
                    {
                        if (collider.ID != OwnerId)
                        {
                            collider.HandleHitBySkill(this);
                            FireUsedWeaponEvent(collider);
                        }
                    }
                }
            }
        }
    }
}
