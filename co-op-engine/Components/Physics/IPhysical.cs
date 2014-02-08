using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Physics
{
    public interface IPhysical
    {
        Rectangle BoundingBox { get; }
    }
}
