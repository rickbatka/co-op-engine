using co_op_engine.Collections;
using co_op_engine.Components.Rendering;
using co_op_engine.Rendering;
using co_op_engine.Utility;
using co_op_engine.Utility.Camera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Rendering
{
    public class RenderBase
    {
        protected readonly IRenderable owner;

        public AnimationSet animationSet;

        public Animation CurrentAnimation { get { return animationSet.CurrentAnimatedRectangle; } }

        public RenderBase(IRenderable owner, Texture2D texture, AnimationSet animationSet)
        {
            this.owner = owner;
            this.owner.Texture = texture;
            this.animationSet = animationSet;
        }

        virtual public void Update(GameTime gameTime)
        {
            animationSet.currentState = (int)owner.CurrentState;
            animationSet.currentFacingDirection = (int)owner.FacingDirection;
            animationSet.Update(gameTime);
            owner.CurrentFrame = CurrentAnimation.CurrentFrame;
        }

        virtual public void Draw(SpriteBatch spriteBatch)
        {
            if (!owner.Visible) { return; }

            spriteBatch.Draw(
                owner.Texture,
                owner.Position.Rounded(),
                owner.CurrentFrame.SourceRectangle,
                GetSpriteDrawColor(),
                owner.FullyRotatable ? owner.RotationTowardFacingDirectionRadians : 0f,
                GetCenterOrigin(),
                owner.Scale,
                SpriteEffects.None,
                owner.Position.Y / Camera.Instance.ViewBoundsRectangle.Bottom
            );
        }

        private Color GetSpriteDrawColor()
        { 
            if(owner.CurrentState == Constants.ACTOR_STATE_BEING_HURT)
            {
                return Color.Red;
            }
            return Color.White;
        }

        virtual public void DebugDraw(SpriteBatch spriteBatch)
        {

            //the full curent render box
            spriteBatch.Draw(
                texture: AssetRepository.Instance.PlainWhiteTexture,
                position: GetDrawTarget(),
                sourceRectangle: owner.CurrentFrame.SourceRectangle,
                color: new Color(Color.Green, 0.01f),
                rotation: owner.FullyRotatable ? owner.RotationTowardFacingDirectionRadians : 0f,
                origin: GetCenterOrigin(),
                scale: owner.Scale,
                effect: SpriteEffects.None,
                depth: 0f);

            // draw the damage dots
            var damageDots = CurrentAnimation.CurrentFrame.DamageDots;

            if (damageDots != null && damageDots.Length > 0)
            {
                foreach (var dot in damageDots)
                {
                    spriteBatch.Draw(
                        texture: AssetRepository.Instance.PlainWhiteTexture,
                        position: DrawingUtility.GetAbsolutePosition(owner, dot.Location),
                        sourceRectangle: null,
                        color: Color.Black,
                        rotation: 0f,
                        origin: Vector2.Zero,
                        scale: 1f,
                        effect: SpriteEffects.None,
                        depth: 1f
                    );
                }
            }
        }

        private Vector2 GetDrawTarget()
        {
            //return new Rectangle(
            //    x: (int)(owner.Position.X),
            //    y: (int)(owner.Position.Y),
            //    width: owner.CurrentFrame.DrawRectangle.Width,
            //    height: owner.CurrentFrame.DrawRectangle.Height
            //);
            return owner.Position.Rounded();
        }

        protected Vector2 GetCenterOrigin()
        {
            var center = new Vector2(
                x: owner.CurrentFrame.SourceRectangle.Width / 2f,
                y: owner.CurrentFrame.SourceRectangle.Height / 2f
            );
            
            return center;
        }

        internal void ReceiveCommand(Networking.Commands.GameObjectCommand command)
        {
            throw new NotImplementedException("THIS DOES NOTHING YET");
        }
    }
}
