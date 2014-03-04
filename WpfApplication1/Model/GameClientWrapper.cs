using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using co_op_engine.Networking;
using System.Xml.Serialization;

namespace DevTools.Model
{
    class GameClientWrapper
    {
        public NetworkClient client;
        public NetworkServer server;

        Action<string> messageHandler;

        public GameClientWrapper(Action<string> handleMessage)
        {
            messageHandler = handleMessage;
        }

        public void SpinUpServer()
        {
            server = new NetworkServer();
            server.StartHosting();
        }

        public void SpinUpClient(string ip = "127.0.0.1")
        {
            client = new NetworkClient();
            client.DEBUGNETWORKTRAFFIC += HandleMessageTraffic;
            client.ConnectToGame(ip);
        }

        private void HandleMessageTraffic(object traffic, EventArgs nothing)
        {
            messageHandler((string)traffic);
        }
    }
}
