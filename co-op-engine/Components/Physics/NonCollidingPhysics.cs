using co_op_engine.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Physics
{
    class NonCollidingPhysics : PhysicsBase, IPhysical
    {

        public NonCollidingPhysics(GameObject owner)
            : base(owner)
        { }

        override public void Update(GameTime gameTime)
        { }
        override public void Draw(SpriteBatch spriteBatch)
        { }

    }
}
