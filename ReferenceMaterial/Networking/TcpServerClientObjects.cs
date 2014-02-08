using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReferenceMaterial.Networking
{
    class BasicClient
    {
        public event EventHandler OnReceiveMessage;
        public event EventHandler OnConnected;
        public event EventHandler OnServerLostConnect;
        public event EventHandler OnNetworkError;

        TcpClient client;
        IPEndPoint serverEndPoint;


        public BasicClient(string IP, int port)
        {
            client = new TcpClient();
            client.Client.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);
            serverEndPoint = new IPEndPoint(IPAddress.Parse(IP), port);
        }

        public void StartConnect()
        {
            Thread tryconnectTHread = new Thread(new ThreadStart(Connect));
            tryconnectTHread.IsBackground = true;
            tryconnectTHread.Start();
        }

        private void Connect()
        {
            try
            {
                client.Connect(serverEndPoint);
                if (OnConnected != null)
                {
                    OnConnected(this, null);
                }
                Thread clientThread = new Thread(new ThreadStart(ClientHandleInput));
                clientThread.IsBackground = true;
                clientThread.Start();
            }
            catch
            { }
        }

        public void Disconnect()
        {
            try
            {
                //probably should send disconnect object to alert server to clean up
                client.Close();
            }
            catch
            {
                if (OnNetworkError != null)
                {
                    OnNetworkError(this, null);
                }
            }
        }

        public void SendTransmission(object transmission)
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                NetworkStream stream = client.GetStream();
                formatter.Serialize(stream, transmission);
            }
            catch
            {
                if (OnNetworkError != null)
                {
                    OnNetworkError(this, null);
                }
            }
        }

        public void ClientHandleInput()
        {
            try
            {
                NetworkStream stream = client.GetStream();
                BinaryFormatter formatter = new BinaryFormatter();
#warning refactor while to be conditional on connection, but more reliable than client.connected
                while (true)
                {
                    object transmission = formatter.Deserialize(stream);
                    if (OnReceiveMessage != null)
                    {
                        OnReceiveMessage(transmission, null);
                    }
                }
            }
            catch
            {
                if (OnNetworkError != null)
                {
                    OnNetworkError(this, null);
                }
            }

            try
            {
                client.Close();
            }
            catch
            {
                //not an error, this happens if the socket was cleaned up normally and couldn't close...
            }
            if (OnServerLostConnect != null)
            {
                OnServerLostConnect(this, null);
            }
        }
    }


    class BaseServer
    {
        private TcpListener tcpListener;
        private Thread listenThread;
        private List<TcpClient> clients;

        public event EventHandler OnReceiveMessage;
        public event EventHandler OnClientConnect;
        public event EventHandler OnClientDisconnect;
        public event EventHandler OnNetworkError;

        public BaseServer(int port)
        {
#warning this can throw and should be refactored to handle it... may not be a good idea in constructor.
            this.tcpListener = new TcpListener(IPAddress.Any, port);
            this.clients = new List<TcpClient>();
        }

        public void StartListening()
        {
            this.listenThread = new Thread(new ThreadStart(ListenForClients));
            this.listenThread.IsBackground = true;
            this.listenThread.Start();
        }

        public void StopListening()
        {
            try
            {
                this.tcpListener.Stop();
            }
            catch
            {
                if (OnNetworkError != null)
                {
                    OnNetworkError(this, null);
                }
            }
        }

        private void ListenForClients()
        {
            try
            {
                this.tcpListener.Start();

#warning this should be conditional at some point.... somehow, networks can suck sometimes...
                while (true)
                {
                    var client = this.tcpListener.AcceptTcpClient();
                    client.Client.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);

                    clients.Add(client);

                    if (OnClientConnect != null)
                    {
                        OnClientConnect(this, null);
                    }

                    Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                    clientThread.IsBackground = true;
                    clientThread.Start(client);
                }
            }
            catch
            {
                if (OnNetworkError != null)
                {
                    OnNetworkError(this, null);
                }
            }
        }

        public void SendTransmissionToAll(object transmission)
        {
            foreach (var client in clients)
            {
                try
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    NetworkStream stream = client.GetStream();
                    formatter.Serialize(stream, transmission);
                }
                catch
                {
                    if (OnNetworkError != null)
                    {
                        OnNetworkError(this, null);
                    }
                }
            }
        }

        private void HandleClientComm(object client)
        {
            TcpClient tcpClient = (TcpClient)client;
            try
            {
                NetworkStream clientStream = tcpClient.GetStream();
                BinaryFormatter formatter = new BinaryFormatter();

#warning another loop that should be conditional, this one might even spin out of control in certain situations...
                while (true)
                {
                    object transmission = formatter.Deserialize(clientStream);
                    if (OnReceiveMessage != null)
                    {
                        OnReceiveMessage(transmission, null);
                    }
                }
            }
            catch
            {
                if (OnNetworkError != null)
                {
                    OnNetworkError(this, null);
                }
            }

            if (OnClientDisconnect != null)
            {
                OnClientDisconnect(this, null);
            }

            try
            {
                tcpClient.Close();
                tcpListener.Stop();
            }
            catch
            {
                //not neccessarily an error... sometimes due to overzealous socket recycling
            }

            if (OnClientDisconnect != null)
            {
                OnClientDisconnect(this, null);
            }
        }

        public void DropAllClientsAndCloseDown()
        {
            try
            {
                this.tcpListener.Stop();
            }
            catch
            {
                if (OnNetworkError != null)
                {
                    OnNetworkError(this, null);
                }
            }

            foreach (var client in clients)
            {
                try
                {
#warning should add disconnect object signal here for client notifiaction
                    client.Close();
                }
                catch
                {
                    if (OnNetworkError != null)
                    {
                        OnNetworkError(this, null);
                    }
                }
            }
            clients.Clear();
        }
    }
}
