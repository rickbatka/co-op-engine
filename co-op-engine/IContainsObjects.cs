using co_op_engine.World.Level;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine
{
    public interface IContainsObjects
    {
        ObjectContainer Container { get; }
    }
}
