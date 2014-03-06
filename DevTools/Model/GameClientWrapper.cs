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
            messageHandler("Spinning up server");
            server = new NetworkServer();
            server.StartHosting();
        }

        public void SpinUpClient(string ip = "127.0.0.1")
        {
            messageHandler(string.Format("Connecting to {0}", ip));
            client = new NetworkClient();
            client.DEBUGNETWORKTRAFFIC += HandleMessageTraffic;
            client.OnNetworkError += HandleNetworkError;
            client.OnServerConnected += client_OnServerConnected;
            client.ConnectToGame(ip);
        }

        void client_OnServerConnected(NetworkClient sender, InitialNetworkData data)
        {
            string names = "";

            if (data.PlayerNames != null)
            {
                foreach (var s in data.PlayerNames)
                {
                    names += " " + s;
                }
            }

            messageHandler(
                string.Format("MaxPlayers:{0}, myid:{1}, others:{3}",
                    data.MaxPlayers,
                    data.PlayerId,
                    data.PlayerName,
                    names));
        }

        private void HandleMessageTraffic(object traffic, EventArgs nothing)
        {
            messageHandler((string)traffic);
        }

        private void HandleNetworkError(object sender, EventArgs e)
        {
            messageHandler(((Exception)sender).Message);
        }

        internal void StopServer()
        {
            messageHandler("Stopping server");
            server.StopHostingAndCleanup();
        }

        internal void StopClient()
        {
            messageHandler("Stopping client");
            client.DisconnectFromGame();
        }
    }
}
