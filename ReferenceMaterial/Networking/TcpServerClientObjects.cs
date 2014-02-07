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
            }
        }

        public void ClientHandleInput()
        {
            try
            {
                NetworkStream stream = client.GetStream();
                BinaryFormatter formatter = new BinaryFormatter();
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
            }

            try
            {
                client.Close();
            }
            catch
            { }
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

        private TcpClient client;

        public event EventHandler OnReceiveMessage;
        public event EventHandler OnClientConnect;
        public event EventHandler OnClientDisconnect;

        public BaseServer(int port)
        {
            this.tcpListener = new TcpListener(IPAddress.Any, port);
        }

        public void StartListening()
        {
            this.listenThread = new Thread(new ThreadStart(ListenForClients));
            this.listenThread.IsBackground = true;
            this.listenThread.Start();
        }

        private void ListenForClients()
        {
            this.tcpListener.Start();

            client = this.tcpListener.AcceptTcpClient();
            client.Client.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);
            if (OnClientConnect != null)
            {
                OnClientConnect(this, null);
            }

            Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
            clientThread.IsBackground = true;
            clientThread.Start(client);
        }

        public void SendTransmission(object transmission)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            NetworkStream stream = client.GetStream();
            formatter.Serialize(stream, transmission);
        }

        private void HandleClientComm(object client)
        {
            TcpClient tcpClient = (TcpClient)client;
            NetworkStream clientStream = tcpClient.GetStream();
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
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
            catch { }

        }
    }
}
