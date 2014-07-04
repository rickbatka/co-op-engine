using co_op_engine.Collections;
using co_op_engine.Components.Rendering;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Skills
{
    public abstract partial class Skill : IRenderable
    {
        protected GameObject Owner;
        protected SkillsComponent SkillsComponent;
        private RenderBase Renderer;
        protected SpacialBase CurrentQuad { get { return Owner.CurrentQuad; } }

        protected List<GameObject> HitObjectsList;

        public int ID;
        public int OwnerId;

        public int CurrentState { get { return Owner.CurrentState; } set { Owner.CurrentState = value; } }
        public ActorState CurrentStateProperties { get { return ActorStates.States[CurrentState]; } }

        public bool Visible { get; set; }
        public int Team { get { return Owner.Team; } }
        public Frame CurrentFrame { get; set; }
        public Texture2D Texture { get; set; }
        public Vector2 Position { get { return new Vector2(Owner.Position.X, Owner.Position.Y - 1);  }}
        public int FacingDirection { get { return Owner.FacingDirection; } }
        public float RotationTowardFacingDirectionRadians { get { return Owner.RotationTowardFacingDirectionRadians; } }
        public float Scale { get; set; }


        public Skill(SkillsComponent skillsComponent, GameObject owner)
        {
            this.SkillsComponent = skillsComponent;
            this.Owner = owner;
            this.ID = MechanicSingleton.Instance.GetNextObjectCountValue();
            this.OwnerId = owner.ID;
            this.Visible = true;
            this.Scale = owner.Scale;
            HitObjectsList = new List<GameObject>();
        }

        protected bool HasntBeenHit(Object check)
        {
            return !HitObjectsList.Contains(check);
        }

        public void SetRenderer(RenderBase renderer)
        {
            this.Renderer = renderer;
        }

        protected abstract void UseSkill(int attackTimer = 0);
        protected abstract void UpdateState(GameTime gameTime);
        protected abstract void SkillHitObject(GameObject receiver);
        public abstract void Activate(int attackTimer = 0);
        
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
            if(this.Team == receiver.Team)
            {
                Owner.FireOnUsedWeaponEffectOnFriendly(this, null);
            }
            else
            {
                Owner.FireOnDidAttackNonFriendly(this, null);
            }
        }

        public bool FullyRotatable 
        { 
            get 
            {
                if (CurrentState == Constants.ACTOR_STATE_ATTACKING)
                {
                    return true;
                }
                return false;
            } 
        }

        public void ResetSkill(int state, int facingDirection)
        {
            if (Renderer != null)
            {
                Renderer.animationSet.GetAnimationFallbackToDefault(state, facingDirection).Reset();
            }

            HitObjectsList.Clear();
        }

        protected virtual void QueryForHits()
        {
            if (CurrentFrame.DamageDots != null)//doesn't need to be attacking anymore
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
                            SkillHitObject(collider);
                            HitObjectsList.Add(collider);
                            FireUsedWeaponEvent(collider);//TODO should fire successful hit event to director
                        }
                    }
                }
            }
        }
    }
}
