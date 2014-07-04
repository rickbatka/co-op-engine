using co_op_engine.Components.Brains.AI;
using co_op_engine.Components.Particles;
using co_op_engine.Components.Skills;
using co_op_engine.Components.Skills.StatusEffects;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Combat
{
    /// <summary>
    /// responsible for ?
    /// telling player when they are dead (could live in engine)
    /// kicking off death animation (could live in engine)
    /// tracking status effects (could go to skills, brain, or engine)
    /// a few minor utility operations. (related to dying and drawing, moves with death stuff)
    /// </summary>
    public class CombatBase
    {
        private List<StatusEffectBase> CurrentStatusEffects;
        protected GameObject owner;
        private TimeSpan dyingAnimationTimer;

        public CombatBase(GameObject owner)
        {
            this.owner = owner;
            CurrentStatusEffects = new List<StatusEffectBase>();
        }

        virtual public void Update(GameTime gameTime)
        {
            //TODO move to appropriate area
            MaybeStartDyingFromWounds();

            foreach (var status in CurrentStatusEffects)
            {
                status.Update(gameTime);
            }
            CurrentStatusEffects.RemoveAll(uu => uu.Done);
        }

        virtual public void Draw(SpriteBatch spriteBatch) { }

        private void MaybeStartDyingFromWounds()
        {
            if (owner.Health <= 0)
            {
                if (owner.CurrentStateProperties.CanStartDying)
                {
                    owner.CurrentState = Constants.ACTOR_STATE_DYING;
                    dyingAnimationTimer = TimeSpan.FromMilliseconds(owner.Renderer.animationSet.GetAnimationDuration(Constants.ACTOR_STATE_DYING, owner.FacingDirection));

                    // animate dying for x milliseconds, then kill
                    GameTimerManager.Instance.SetTimer(
                        time: owner.Renderer.animationSet.GetAnimationDuration(Constants.ACTOR_STATE_DYING, owner.FacingDirection),
                        updateCallback: (t) => { },
                        endCallback: (t) => 
                        { 
                            HandleDeath();
                        }
                    );
                }
            }
        }

        private void HandleDeath()
        {
            owner.Brain = new DoNothingBrain(owner);
            owner.InputMovementVector = Vector2.Zero;
            owner.CurrentState = Constants.ACTOR_STATE_DEAD;
            owner.CurrentQuad.Remove(owner);
        }

        virtual public void DebugDraw(SpriteBatch spriteBatch)
        {
            // object debug info
            spriteBatch.DrawString(
                spriteFont: AssetRepository.Instance.Arial,
                text: owner.Health + "/" + owner.Health.MaxValue,
                position: PositionAboveHead(25),
                color: Color.White,
                rotation: 0f,
                origin: Vector2.Zero,
                scale: 1f,
                effects: SpriteEffects.None,
                depth: 1f
            );

            spriteBatch.DrawString(
                spriteFont: AssetRepository.Instance.Arial,
                text: owner.DisplayName,
                position: PositionAboveHead(50),
                color: Color.White,
                rotation: 0f,
                origin: Vector2.Zero,
                scale: 1f,
                effects: SpriteEffects.None,
                depth: 1f
            );
        }

        private Vector2 PositionAboveHead(int distance)
        {
            var aboveHead = new Vector2(
                x: owner.Position.X - (owner.CurrentFrame.DrawRectangle.Width / 2f),
                y: owner.Position.Y - (owner.CurrentFrame.DrawRectangle.Height / 2f) - distance
            );

            return aboveHead;
        }

        internal void ReceiveCommand(Networking.Commands.GameObjectCommand command)
        {
            throw new NotImplementedException();
        }

        public void ApplyStatusEffect(StatusEffectBase statusEffect)
        {
            //TODO possibly some logic on whether or not it can be applied here or not, still deciding responsibilities since skills are now on the skill to check the object not the other way around
            statusEffect.Start();
            CurrentStatusEffects.Add(statusEffect);
        }
    }
}
