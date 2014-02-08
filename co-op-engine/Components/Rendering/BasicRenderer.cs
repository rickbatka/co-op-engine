using co_op_engine.Components.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Rendering
{
    class BasicRenderer : RenderBase, IRenderable
    {
        public BasicRenderer(GameObject owner, Texture2D tex)
            : base(owner)
        {
            _texture = tex;
        }

        public override void Update(GameTime gameTime)
        {
            //flat sprite, nothing special here
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, GetDrawTarget(), Color.White);
        }

        private Rectangle GetDrawTarget()
        {
            return new Rectangle(
                x: (int)owner.Position.X,
                y: (int)owner.Position.Y,
                width: owner.Width,
                height: owner.Height
            );
        }
    }
}
