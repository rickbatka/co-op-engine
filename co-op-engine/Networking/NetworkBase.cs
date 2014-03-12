using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using co_op_engine.Collections;

namespace co_op_engine.Networking
{
    abstract public class NetworkBase
    {
        abstract public event EventHandler OnNetworkError;

        abstract public bool IsHosting { get; }
        abstract public ThreadSafeBuffer<CommandObject> Input { get; }
        abstract public ThreadSafeBuffer<CommandObject> Output { get; }
    }
}
