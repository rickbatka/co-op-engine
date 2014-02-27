using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace co_op_engine.Networking
{
    struct GameClient
    {
        public int ClientId;
        public TcpClient Client;
    }
}
