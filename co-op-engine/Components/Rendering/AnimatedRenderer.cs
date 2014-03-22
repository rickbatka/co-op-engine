using co_op_engine.Collections;
using co_op_engine.Rendering;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace co_op_engine.Components.Rendering
{
    /// <summary>
    /// moves things and has an object state that updates based on direction, movement etc
    /// </summary>
    public class AnimatedRenderer : RenderBase
    {
        public AnimationSet animationSet;

        public Animation CurrentAnimation { get { return animationSet.CurrentAnimatedRectangle; } }

        public AnimatedRenderer(IRenderable owner, Texture2D texture, AnimationSet animationSet)
            :base(owner, texture)
        {
            this.animationSet = animationSet;
        }

        public override void Update(GameTime gameTime)
        {
            animationSet.currentState = (int)owner.CurrentState;
            animationSet.currentFacingDirection = (int)owner.FacingDirection;
            animationSet.Update(gameTime);
            owner.CurrentFrame = CurrentAnimation.CurrentFrame;

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void DebugDraw(SpriteBatch spriteBatch)
        {
            base.DebugDraw(spriteBatch);

#warning bad
            var damageDots = CurrentAnimation.CurrentFrame.DamageDots;

            if (damageDots != null && damageDots.Length > 0)
            {
                foreach (var dot in damageDots)
                {
                    spriteBatch.Draw(
                        texture: AssetRepository.Instance.PlainWhiteTexture,
                        position: DrawingUtility.GetAbsolutePosition(owner, dot.Location),
                        color: Color.Black
                    );
                }
            }
        }       
    }
}
