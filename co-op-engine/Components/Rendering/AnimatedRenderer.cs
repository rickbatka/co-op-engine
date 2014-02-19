using co_op_engine.Collections;
using co_op_engine.Events;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace co_op_engine.Components.Rendering
{
    /// <summary>
    /// moves things and has an object state that updates based on direction, movement etc
    /// </summary>
    class AnimatedRenderer : RenderBase
    {
        int currentState; // will be enumerated later
        int currentDirection;
        private AnimationSet animationSet;

        public AnimatedRenderer(GameObject owner, Texture2D texture, AnimationSet animationSet)
            :base(owner, texture)
        {
            currentState = AnimationSet.ANIM_STATE_DEFAULT_IDLE_SOUTH;
            currentDirection = Constants.South;
            this.animationSet = animationSet;
        }

        public void HandleStateChange(object sender, EventArgs args)
        {
            animationSet.currentState = (int) ((ActorStateChangedEventArgs)args).NewState;
        }

        public void HandleDirectionChange(object sender, EventArgs args)
        {
            animationSet.currentFacingDirection = (int)((ActorDirectionChangedEventArgs)args).NewDirection;
        }

        public override void Update(GameTime gameTime)
        {
            animationSet.Update(gameTime);
            currentDrawRectangle = animationSet.GetCurrentAnimationRectangle().CurrentDrawRectangle;
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw()
            base.Draw(spriteBatch);
        }
    }
}
