using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Skills.Rages
{
    /// <summary>
    /// defines the specific rage explosion logic 
    /// of checking the radius
    /// </summary>
    public class RageExplosion : Rage
    {
        public int RageCost;//subclassThing
        protected int Radius = 140;//too specific, put in main class
        protected RadiusProximityChecker RadiusChecker;

        public RageExplosion(int cost, SkillsComponent skillsComponent, GameObject owner)
            :base(skillsComponent, owner, cost)
        {
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

        public override void Activate(int attackTimer = 0)
        {
            var time = GetAnimationDuration(Constants.ACTOR_STATE_RAGING, Owner.FacingDirection) + 2000; 
            UseSkill(time);
        }

        override protected void QueryForHits()
        {
            if (CurrentState == Constants.ACTOR_STATE_RAGING)
            {
                var colliders = RadiusChecker.QueryRange();
                foreach (var collider in colliders)
                {
                    if (collider.Team != Owner.Team)
                    {
                        SkillHitObject(collider);
                        HitObjectsList.Add(collider); //TODO could move this to it's own base method since these are all the same everywhere
                        FireUsedWeaponEvent(collider);//DOTHIS this is backwards?
                    }
                }
            }
        }

        protected override void SkillHitObject(GameObject receiver)
        {
            if (HasntBeenHit(receiver))
            {
                AddDamageOverTime(receiver, 8000, 1000, 5);
            }
        }
    }
}
