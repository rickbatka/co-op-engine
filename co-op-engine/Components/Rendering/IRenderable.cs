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
        Texture2D TextureProp { get; set; }
        Vector2 PositionProp { get; }
        int WidthProp { get; }
        int HeightProp { get; }
        ActorState CurrentActorStateProp { get; }
        int FacingDirectionProp { get; }
    }
}
