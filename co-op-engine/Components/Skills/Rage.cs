using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Skills
{
    public class Rage : Skill
    {
        private TimeSpan currentRageTimer;
        protected int RageMeter = 0;
        protected int Cost;
        protected int Radius = 140;
        protected RadiusProximityChecker RadiusChecker;

        public Rage(SkillsComponent skillsComponent, GameObject owner, int cost)
            : base(skillsComponent, owner)
        {
            Cost = cost;
            RadiusChecker = new RadiusProximityChecker(owner, Radius);
        }

        public override void DebugDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                texture: AssetRepository.Instance.PlainWhiteTexture, 
                destinationRectangle: RadiusChecker.DrawArea.ToRectangle(), 
                sourceRectangle: null,
                color: Color.White,
                rotation: 0f,
                origin: Vector2.Zero,
                effect: SpriteEffects.None,
                depth: 1f);
        }

        protected override void UpdateState(GameTime gameTime)
        {
            if (CurrentState == Constants.ACTOR_STATE_RAGING)
            {
                currentRageTimer -= gameTime.ElapsedGameTime;
                if (currentRageTimer <= TimeSpan.Zero)
                {
                    CurrentState = Constants.ACTOR_STATE_IDLE;
                    currentRageTimer = TimeSpan.Zero;
                    ResetAnimation(Constants.ACTOR_STATE_RAGING, Owner.FacingDirection);
                }
            }
        }

        override public bool TryInitiateSkill(int attackTimer = 0)
        {
            if (!CurrentStateProperties.CanInitiateSkills) { return false; }
            if (RageMeter < Cost) { return false; }

            UseSkill(attackTimer);

            return true;
        }

        override protected void UseSkill(int attackTimer = 0)
        {
            if (attackTimer == 0)
            {
                attackTimer = GetAnimationDuration(Constants.ACTOR_STATE_RAGING, Owner.FacingDirection);
            }
            currentRageTimer = TimeSpan.FromMilliseconds(attackTimer);
            CurrentState = Constants.ACTOR_STATE_RAGING;
        }

        override protected void QueryForHits()
        {
            if (CurrentState == Constants.ACTOR_STATE_RAGING)
            {
                var colliders = RadiusChecker.QueryRange();
                foreach (var collider in colliders)
                {
                    if (!collider.Friendly)
                    {
                        collider.HandleHitBySkill(this);
                        FireUsedWeaponEvent(collider);
                    }
                }
            }
        }
    }
}
