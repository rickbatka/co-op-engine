﻿using co_op_engine.Collections;
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
        private AnimationSet animationSet;

        public AnimatedRenderer(IRenderable owner, Texture2D texture, AnimationSet animationSet)
            :base(owner, texture)
        {
            this.animationSet = animationSet;
        }

        public void HandleStateChange(co_op_engine.Components.Brains.BrainBase sender, co_op_engine.Components.Brains.ActorStateChangedEventArgs data)
        {
            
        }

        public void HandleDirectionChange(co_op_engine.Components.Physics.PhysicsBase sender, co_op_engine.Components.Physics.ActorDirectionChangedEventArgs directionData)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            animationSet.currentState = (int)owner.CurrentActorStateProp;
            animationSet.currentFacingDirection = (int)owner.FacingDirectionProp;
            animationSet.Update(gameTime);
            currentDrawRectangle = animationSet.GetCurrentAnimationRectangle().CurrentDrawRectangle;
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
