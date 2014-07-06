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
    /// <summary>
    /// purpose: pure data and very VERY basic helper methods
    ///     for core functionality.
    /// 
    /// This should not be changed or added to without consideration
    ///     to every skill in the codebase, most likely
    ///     it should end up in the higher level skill implementations
    /// </summary>
    public abstract partial class SkillBase : IRenderable
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
        public Vector2 Position { get { return new Vector2(Owner.Position.X, Owner.Position.Y - 1); } }
        public int FacingDirection { get { return Owner.FacingDirection; } }
        public float RotationTowardFacingDirectionRadians { get { return Owner.RotationTowardFacingDirectionRadians; } }
        public float Scale { get; set; }


        public SkillBase(SkillsComponent skillsComponent, GameObject owner)
        {
            this.SkillsComponent = skillsComponent;
            this.Owner = owner;
            this.ID = MechanicSingleton.Instance.GetNextObjectCountValue();
            this.OwnerId = owner.ID;
            this.Visible = true;
            this.Scale = owner.Scale;
            HitObjectsList = new List<GameObject>();
        }

        /// <summary>
        /// checks to see if the object in question hasn't been hit by this skill
        /// during the current activation. it automatically resets via the ResetSkill
        /// method following the normal pattern
        /// </summary>
        protected bool HasntBeenHit(Object check)
        {
            return !HitObjectsList.Contains(check);
        }
        public void SetRenderer(RenderBase renderer)
        {
            this.Renderer = renderer;
        }

        /// <summary>
        /// Starts the underlying skill pattern for the specific skill,
        /// generally called from The Activate method
        /// </summary>
        protected abstract void UseSkill(int attackTimer = 0);
        /// <summary>
        /// Called during the update loop, meant to handle the state
        /// changes at the skill mechanic level, possibly overridden by
        /// specific skills if they deviate from the underlying pattern
        /// e.g. a boost that does damage
        /// </summary>
        protected abstract void UpdateState(GameTime gameTime);
        /// <summary>
        /// generally overridden in the concrete implementation of a skill
        /// this is what is invoked when the skill's damage dots hit something
        /// </summary>
        protected abstract void SkillHitObject(GameObject receiver);
        /// <summary>
        /// The public entry method for initiating a skill to start
        /// </summary>
        public abstract void Activate(int attackTimer = 0);

        public virtual void Update(GameTime gameTime)
        {
            UpdateState(gameTime);
            QueryForHits();

            if (Renderer != null)
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
            if (this.Team == receiver.Team)
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
            if (CurrentFrame.DamageDots != null)
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
