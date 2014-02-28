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
        public string ClientName;
    }

    struct InitialNetworkData
    {
        public string PlayerName;
        public int PlayerId;
        public int MaxPlayers;
        public string[] PlayerNames;
    }
}
