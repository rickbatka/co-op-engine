using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace co_op_engine.Networking
{
    /// <summary>
    /// An identification object meant to  specify what networkId this client is
    /// as well as the name and connection it holds
    /// </summary>
    struct GameClient
    {
        public int ClientId;
        public TcpClient Client;
        public string ClientName;
    }

    /// <summary>
    /// Initial data that is transferred over the network to establish both the 
    /// handshake and setup the player and their wold on both parties
    /// </summary>
    [Serializable]
    public struct InitialNetworkData
    {
        public string PlayerName;
        public int PlayerId;
        public int MaxPlayers;
        public string[] PlayerNames;
    }
}
