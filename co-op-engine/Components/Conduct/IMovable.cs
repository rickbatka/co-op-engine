using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Conduct
{
    interface IMovable
    {
        Vector2 MovementDirection { get; }
        Vector2 Position { get; }
        int Width { get; }
        int Height { get; }
    }
}
