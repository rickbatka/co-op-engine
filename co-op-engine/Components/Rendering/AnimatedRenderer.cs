using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using co_op_engine.Collections;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace co_op_engine.Components.Rendering
{
    /// <summary>
    /// moves things and has an object state that updates based on direction, movement etc
    /// </summary>
    class AnimatedRenderer : RenderBase
    {
        int currentState; // will be enumerated later
        private AnimationSet tileSheet;

        public AnimatedRenderer(GameObject owner, Texture2D texture, AnimationSet tileSheet)
            :base(owner, texture)
        {
            currentState = 0;
            this.tileSheet = tileSheet;
        }

        private void HandleStateChange(GameObject sender, int state)
        {
            //we can worry about the sender later, maybe leave it open so they can have a unified dance ai, who knows ^_^
            currentState = state;
        }

        public override void Update(GameTime gameTime)
        {
            tileSheet.Update(gameTime);
            currentDrawRectangle = tileSheet.GetCurrentAnimationRectangle().CurrentDrawRectangle;
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw()
            base.Draw(spriteBatch);
        }
    }
}
