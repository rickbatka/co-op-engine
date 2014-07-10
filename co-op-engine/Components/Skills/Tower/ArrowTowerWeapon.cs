using co_op_engine.Components.Brains.TowerBrains;
using co_op_engine.Factories;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using co_op_engine.Components.Skills.Weapons;
using co_op_engine.Components.Brains.Projectiles;
using co_op_engine.Components.Rendering;

namespace co_op_engine.Components.Skills.Tower
{
    public class ArrowTowerWeapon : SkillBase
    {
        public int TowerShootIntervalMilli;
        private TimeSpan TowerShootTimer;

        public ArrowTowerWeapon(SkillsComponent skillsComponent, GameObject owner, int shootTimeMilli)
            : base(skillsComponent, owner)
        {
            TowerShootIntervalMilli = shootTimeMilli;
            TowerShootTimer = TimeSpan.Zero;
        }

        protected override void UseSkill(int attackTimer = 0)
        {
            var target = ((ArrowTowerBrain)Owner.Brain).Target;
            TowerShootTimer = TimeSpan.FromMilliseconds(TowerShootIntervalMilli);
            
            var projectile = ProjectileFactory.Instance.GetGenericProjectileNoWeapon(Owner, Owner.Position, 1f, AssetRepository.Instance.ArrowTexture, AssetRepository.Instance.ArrowAnimations(1f), 3000);
            projectile.Skills.SetWeapon(new ArrowWeapon(projectile.Skills,projectile));
            projectile.Skills.WeaponSkill.SetRenderer((new RenderBase(projectile.Skills.WeaponSkill, AssetRepository.Instance.ArrowTexture, AssetRepository.Instance.ArrowAnimations(1f))));

            var move = target.Position - Owner.Position;
            move.Normalize();

            ((OneHitStraightProjectileBrain)projectile.Brain).Shoot(move);
        }

        protected override void UpdateState(Microsoft.Xna.Framework.GameTime gameTime)
        {
            TowerShootTimer -= gameTime.ElapsedGameTime;
            if (TowerShootTimer <= TimeSpan.Zero && Owner.CurrentState == Constants.ACTOR_STATE_ATTACKING)
            {
                Owner.CurrentState = Constants.ACTOR_STATE_IDLE;
            }
        }

        protected override void SkillHitObject(GameObject receiver)
        {
            //it's not a hit an object sort of skill
        }

        protected override void QueryForHits()
        {
            //don't need to waste the effort
        }

        public override void Activate(int attackTimer = 0)
        {
            if (TowerShootTimer <= TimeSpan.Zero)
            {
                Owner.CurrentState = Constants.ACTOR_STATE_ATTACKING;
                UseSkill();
            }
        }
    }
}
