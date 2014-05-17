using co_op_engine.Components.Input;
using co_op_engine.Utility;
using co_op_engine.Utility.Camera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using co_op_engine.Components.Particles;
using co_op_engine.Components.Particles.Decorators;

namespace co_op_engine.Components.Brains.TowerBrains
{
    public class HealingAOETowerBrain : BasicTowerBrain
    {
        Color TextureColor = new Color(Color.White, 0.001f);
        int healCooldown = 2000;

        public HealingAOETowerBrain(GameObject owner, TowerPlacingInput placingInput)
            : base(owner, placingInput)
        {
            
        }
        override public void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void HandleFriendlyInRange(GameObject collider)
        {
            base.HandleFriendlyInRange(collider);

            if(Owner.CurrentStateProperties.CanInitiatePrimaryAttackState
                && Owner.Weapon.CurrentWeaponStateProperties.CanInitiatePrimaryAttack)
            {
                Owner.Weapon.PrimaryAttack(healCooldown);
                collider.HandleHitByWeapon(Owner.Weapon);
                ParticleEngine.Instance.Add(
                    new LineParticle()
                    {
                        DrawColor = Color.White,
                        Lifetime = TimeSpan.FromMilliseconds(200),
                        Texture = AssetRepository.Instance.FuzzyLazer,
                        width = 40,
                        end = Owner.Position,
                        start = collider.Position
                    }
                );
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

    }
}
