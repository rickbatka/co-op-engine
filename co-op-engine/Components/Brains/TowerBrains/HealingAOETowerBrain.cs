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
        private RectangleFloat Area;
        private int Range = 250;

        public HealingAOETowerBrain(GameObject owner, TowerPlacingInput placingInput)
            : base(owner, placingInput)
        {
            Area = new RectangleFloat(owner.Position.X - (Range / 2), owner.Position.Y - (Range / 2), Range, Range);
        }
        override public void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Area.X = owner.Position.X - (Range / 2);
            Area.Y = owner.Position.Y - (Range / 2);

            if (owner.CurrentStateProperties.CanInitiatePrimaryAttackState)
            {
                HealFriendsWithinRange();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            /*if (owner.CurrentStateProperties.CanInitiatePrimaryAttackState)
            {
                spriteBatch.Draw(
                    texture: AssetRepository.Instance.PlainWhiteTexture,
                    destinationRectangle: Area.ToRectangle(),
                    sourceRectangle: null,
                    color: Color.LightGreen,
                    rotation: 0f,
                    origin: Vector2.Zero,
                    effect: SpriteEffects.None,
                    depth: 0.01f //behind everythign but background hopefully
                );
            }*/
        }

        private void HealFriendsWithinRange()
        {
            var colliders = owner.CurrentQuad.MasterQuery(Area);
            foreach (var collider in colliders)
            {
                if (collider != owner && collider.Friendly)
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
    }
}
