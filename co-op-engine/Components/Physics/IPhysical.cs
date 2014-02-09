using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using co_op_engine.Collections;
using Microsoft.Xna.Framework.Graphics;

namespace co_op_engine.Components.Physics
{
    interface IPhysical
    {
        Rectangle BoundingBox { get; }
        ElasticQuadTree CurrentQuad { get; }

        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
    }
}
