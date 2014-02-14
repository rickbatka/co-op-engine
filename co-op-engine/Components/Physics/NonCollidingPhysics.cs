﻿using co_op_engine.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace co_op_engine.Components.Physics
{
    class NonCollidingPhysics : PhysicsBase, IPhysical
    {

        public NonCollidingPhysics(GameObject owner, ElasticQuadTree tree)
            : base(owner, tree)
        { }

        override public void Update(GameTime gameTime)
        {
            CurrentQuad.NotfyOfMovement(owner);
        }
        override public void Draw(SpriteBatch spriteBatch)
        { }

    }
}
