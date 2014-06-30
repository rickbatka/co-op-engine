using co_op_engine.Components.Particles;
using co_op_engine.Components.Skills;
using co_op_engine.Components.Skills.StatusEffects;
using co_op_engine.Effects;
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
    /// telling player when they are dead
    /// kicking off death animation
    /// place to put skill hit handler and events
    /// a few minor utility operations
    /// 
    /// this could probably be split up into other locations
    /// </summary>
    public class CombatBase
    {
        private List<StatusEffectBase> CurrentStatusEffects;

        protected GameObject owner;
        Dictionary<string, StatusEffect> effectsByWeapon = new Dictionary<string, StatusEffect>();

        private TimeSpan dyingAnimationTimer;

        public CombatBase(GameObject owner)
        {
            this.owner = owner;
            CurrentStatusEffects = new List<StatusEffectBase>();
        }

        virtual public void Update(GameTime gameTime)
        {
            RemoveFinishedWeaponEffects();
            UpdateCurrentWeaponEffects(gameTime);

            MaybeStartDyingFromWounds();

            //update status effects
            foreach (var status in CurrentStatusEffects)
            {
                status.Update(gameTime);
            }
            CurrentStatusEffects.RemoveAll(uu => uu.Done);
        }

        virtual public void Draw(SpriteBatch spriteBatch) { }

        public void HandleHitBySkill(Skill skill)
        {
            foreach(var effect in skill.Effects)
            {
                string hash = GetHash(skill.ID, effect);
                if (owner.CurrentStateProperties.IsVulnerable 
                    && IsAffected(skill, effect)
                    && !effectsByWeapon.ContainsKey(hash))
                {
                    // get the effect from the weapon, set its receiver to this owner, register it in the list of actie effects
                    var newEffect = (StatusEffect)effect.Clone();//HACK: this is so hackey
                    newEffect.SetReceiver(owner);

                    var rotation = owner.Position - skill.Position;
                    rotation.Normalize();
                    newEffect.SetRotationAtTimeOfHit(rotation);
                    
                    effectsByWeapon.Add(hash, newEffect);

                    // Apply the status effect
                    newEffect.Apply();
                    FireWasAffectedEvent(skill, effect);
                }
            }
        }

        private bool IsAffected(Skill skill, StatusEffect effect)
        {
            if(owner.Team == skill.Team)
            {
                return effect.AffectsFriendlies;
            }

            return effect.AffectsNonFriendlies;
        }

        private void FireWasAffectedEvent(Skill skill, StatusEffect effect)
        {
            if (owner.Team == skill.Team)
            {
                owner.FireOnWasAffectedByFriendlyWeapon(this, null);
            }
            else
            {
                owner.FireOnWasAffectedByNonFriendlyWeapon(this, null);
            }
        }

        private void UpdateCurrentWeaponEffects(GameTime gameTime)
        {
            foreach (var effect in effectsByWeapon.Values)
            {
                // Update the status effect
                effect.Update(gameTime);
            }
        }

        private void RemoveFinishedWeaponEffects()
        {
            var effectsToRemove = new List<string>();
            foreach(var key in effectsByWeapon.Keys)
            {
                if(effectsByWeapon[key].IsFinished)
                {
                    effectsToRemove.Add(key);
                }
            }

            int cnt = effectsToRemove.Count();
            for (int i = 0; i < cnt; i++)
            {
                // Clear the status effect
                effectsByWeapon[effectsToRemove[i]].Clear();
                effectsByWeapon.Remove(effectsToRemove[i]);
            }
        }

        private void MaybeStartDyingFromWounds()
        {
            if (owner.Health <= 0)
            {
                owner.Health.Value = 0;

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
                            owner.CurrentState = Constants.ACTOR_STATE_DEAD;
                        }
                    );
                }
            }
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

        private string GetHash(int weaponId, StatusEffect effect)
        {
            return "" + weaponId + "_" + effect.StatusEffectID;
        }

        internal void ReceiveCommand(Networking.Commands.GameObjectCommand command)
        {
            throw new NotImplementedException();
        }

        public void ApplyStatusEffect(StatusEffectBase statusEffect)
        {
            //TODO: possibly some logic on whether or not it can be applied here
            statusEffect.Start();
            CurrentStatusEffects.Add(statusEffect);
        }
    }
}
