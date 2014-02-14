using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.ServiceProviders
{
    public interface IGraphicsInformationProvider
    {
        int GridSize { get; }
        Rectangle ScreenRectangle { get; }
    }
}
