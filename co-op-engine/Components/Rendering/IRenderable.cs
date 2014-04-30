using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Rendering
{
    public interface IRenderable
    {
        Texture2D Texture { get; set; }
        Vector2 Position { get; }
        int CurrentState { get; }
        int FacingDirection { get; set; }
        float RotationTowardFacingDirectionRadians { get; set; }
        bool FullyRotatable { get; }
        Frame CurrentFrame { get; set; }
    }
}
