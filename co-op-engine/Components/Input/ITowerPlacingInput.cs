using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Input
{
    public interface ITowerPlacingInput
    {
        void Update(GameTime gameTime);
        event EventHandler OnPlacementAttempted;
        event EventHandler OnCoordsUpdated;
    }
}
