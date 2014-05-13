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
        private RectangleFloat DrawArea;
        private int Radius = 250;
        Color TextureColor = new Color(Color.White, 0.001f);

        public HealingAOETowerBrain(GameObject owner, TowerPlacingInput placingInput)
            : base(owner, placingInput)
        {
            DrawArea = new RectangleFloat(owner.Position.X - Radius, owner.Position.Y - Radius, Radius*2, Radius*2);
        }
        override public void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            DrawArea.X = Owner.Position.X - Radius;
            DrawArea.Y = Owner.Position.Y - Radius;

            if (Owner.CurrentStateProperties.CanInitiatePrimaryAttackState)
            {
                HealFriendsWithinRange();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        private void HealFriendsWithinRange()
        {
            var colliders = Owner.CurrentQuad.MasterQuery(DrawArea);
            foreach (var collider in colliders)
            {
                if (collider != Owner && collider.Friendly
                    && IsWithinRadius(collider))
                {
                    ParticleEngine.Instance.Add(
                        new LineParticle()
                        {
                            DrawColor = Color.White,
                            Lifetime = TimeSpan.FromMilliseconds(32),
                            Texture = AssetRepository.Instance.FuzzyLazer,
                            width = 40,
                            end = Owner.Position,
                            start = collider.Position
                        });

                    collider.HandleHitByWeapon(Owner.Weapon);
                }
            }
        }

        private bool IsWithinRadius(GameObject collider)
        {
            return (collider.Position - Owner.Position).Length() <= Radius;
        }
    }
}
