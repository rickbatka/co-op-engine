using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using co_op_engine.Collections;
using Microsoft.Xna.Framework.Graphics;

namespace co_op_engine.Components.Rendering
{
    /// <summary>
    /// moves things and has an object state that updates based on direction, movement etc
    /// </summary>
    class AnimatedRenderer : RenderBase, IRenderable
    {
        int currentState; // will be enumerated later
        //some object that holds the information

        public AnimatedRenderer(Texture2D texture, TileSheet tileSheet, GameObject owner)
            :base(owner)
        {
            currentState = 0;
            this._texture = texture;
        }

        private void HandleStateChange(GameObject sender, int state)
        {
            //we can worry about the sender later, maybe leave it open so they can have a unified dance ai, who knows ^_^
            currentState = state;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
        }
    }
}
