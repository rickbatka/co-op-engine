using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Movement
{
    public class SimpleNoStateMover : MoverBase
    {
        public SimpleNoStateMover(GameObject owner)
            : base(owner)
        {
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            //DOTHIS move based on brain's movement vector
            Owner.Position += Owner.InputMovementVector;
            base.Update(gameTime);
        }
    }
}
