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

            DrawArea.X = owner.Position.X - Radius;
            DrawArea.Y = owner.Position.Y - Radius;

            if (owner.CurrentStateProperties.CanInitiatePrimaryAttackState)
            {
                HealFriendsWithinRange();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (owner.CurrentStateProperties.CanInitiatePrimaryAttackState)
            {
                spriteBatch.Draw(
                    texture: AssetRepository.Instance.GreenCircle,
                    destinationRectangle: DrawArea.ToRectangle(),
                    sourceRectangle: null,
                    color: TextureColor,
                    rotation: 0f,
                    origin: Vector2.Zero,
                    effect: SpriteEffects.None,
                    depth: 0.01f //behind everythign but background hopefully
                );
            }
        }

        private void HealFriendsWithinRange()
        {
            var colliders = owner.CurrentQuad.MasterQuery(DrawArea);
            foreach (var collider in colliders)
            {
                if (collider != owner && collider.Friendly
                    && IsWithinRadius(collider))
                {
                    ParticleEngine.Instance.Add(
                        new LineParticle()
                        {
                            DrawColor = Color.White,
                            Lifetime = TimeSpan.FromMilliseconds(100),
                            Texture = AssetRepository.Instance.HealBeam,
                            width = 20,
                            end = owner.Position,
                            start = collider.Position
                        });

                    collider.HandleHitByWeapon(owner.Weapon);
                }
            }
        }

        private bool IsWithinRadius(GameObject collider)
        {
            return (collider.Position - owner.Position).Length() <= Radius;
        }
    }
}
