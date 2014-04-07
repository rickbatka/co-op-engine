using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using co_op_engine.Components;

namespace co_op_engine.Networking
{
    public enum NetworkCommandType
    {
        GameObjectCommand,
    }

    /// <summary>
    /// Serializable command meant to pass functional information over the network
    /// </summary>
    [Serializable]
    public struct NetworkCommandObject
    {
        public int ClientId;
        public NetworkCommandType CommandType;
        public object Command;
    }
}
