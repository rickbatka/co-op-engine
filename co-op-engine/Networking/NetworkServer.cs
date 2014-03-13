using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using co_op_engine.Collections;

namespace co_op_engine.Networking
{
    /// <summary>
    /// Represents the hosts server that clients will connect to
    /// this is where clients talk to each other
    /// </summary>
    public class NetworkServer : NetworkBase
    {
        public override bool IsHosting { get { return false; } }

        const int PORT = 22001;

        public event EventHandler OnClientConnect;
        public event EventHandler OnClientDisconnect;
        public override event EventHandler OnNetworkError;

        private ThreadSafeBuffer<CommandObject> inputBuffer;
        public override ThreadSafeBuffer<CommandObject> Input
        {
            get { return inputBuffer; }
        }

        private ThreadSafeBuffer<CommandObject> outputBuffer;
        public override ThreadSafeBuffer<CommandObject> Output
        {
            get { return outputBuffer; }
        }

        private Thread listenThread;
        private Thread sendThread;
        private List<Thread> clientThreads;
        private List<GameClient> clients;
        private TcpListener listener;
        private int playerIndex;

        public NetworkServer()
        {
            inputBuffer = new ThreadSafeBuffer<CommandObject>();
            outputBuffer = new ThreadSafeBuffer<CommandObject>();
            clientThreads = new List<Thread>();
            clients = new List<GameClient>();

            listenThread = new Thread(new ThreadStart(ListenLoop));
            listenThread.IsBackground = true;

            sendThread = new Thread(new ThreadStart(SendLoop));
            sendThread.IsBackground = true;

            playerIndex = 1;
        }

        public void StartHosting()
        {
            if (listener == null)
            {
                //instantiate listener
                listener = new TcpListener(IPAddress.Any, PORT);
                //start listen loop
                listenThread.Start();
            }
        }

        public void StopHostingAndCleanup()
        {
            listener.Stop();
            listener = null;

            foreach (var thread in clientThreads)
            {
                try
                {
                    thread.Abort();
                }
                catch
                { }
            }

            clientThreads.Clear();

            foreach (var client in clients)
            {
                try
                {
                    client.Client.Close();
                }
                catch
                { }
            }

            clients.Clear();
        }

        public void ListenLoop()
        {
            try
            {
                listener.Start();
                while (true) //an always listening server!!! UNLIMITED PLAYERS!! MWAHAHAHA
                {
                    TcpClient inClient = listener.AcceptTcpClient();
                    inClient.Client.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);

                    var gameClient = new GameClient()
                    {
                        Client = inClient,
                        ClientId = playerIndex++
                    };

                    clients.Add(gameClient);
                    Thread inClientRecvThread = new Thread(new ParameterizedThreadStart(ClientRecvLoop));
                    inClientRecvThread.IsBackground = true;
                    clientThreads.Add(inClientRecvThread);

                    inClientRecvThread.Start(gameClient);
                }
            }
            catch (Exception e)
            {
                if (OnNetworkError != null)
                {
                    OnNetworkError(e, null);
                }
            }
            finally
            {
                try
                {
                    listener.Stop();
                }
                catch
                { }
                listener = null;
            }
        }

        private void SendLoop()
        {
            while (true)
            {
                var queued = inputBuffer.Gather();
                if (queued.Count() == 0)
                {
                    Thread.Sleep(100);
                }
                else if (clients.Count() == 0)
                {
                    Thread.Sleep(600);
                    //eat the list cause nobody cares
                }
                else
                {
                    foreach (var command in queued)
                    {
                        EchoAllOthers(command);
                    }
                }
            }
        }

        public void ClientRecvLoop(object clientObj)
        {
            var client = (GameClient)clientObj;
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                NetworkStream netStream = client.Client.GetStream();

                if (!InitialHandshake(client))
                {
                    throw new Exception("handshake went bad");
                }



                while (true)
                {
                    //blocks here
                    CommandObject command = (CommandObject)formatter.Deserialize(netStream);

                    //send chatter to output
                    EchoAllOthers(command, formatter);

                    outputBuffer.Add(command);
                }
            }
            catch (Exception e)
            {
                if (OnNetworkError != null)
                {
                    OnNetworkError(e, null);
                }
            }
            finally
            {
                try
                {
                    client.Client.Close();

                    lock (clients)
                    {
                        clients.Remove(client);
                    }
                }
                catch
                { }
            }
        }

        private void EchoAllOthers(CommandObject command, BinaryFormatter formatter = null)
        {
            if (formatter == null) formatter = new BinaryFormatter();
            for (int i = 0; i < clients.Count(); ++i)
            {
                if (command.ClientId != clients[i].ClientId)
                {
                    formatter.Serialize(clients[i].Client.GetStream(), command);
                }
            }
        }

        private bool InitialHandshake(GameClient gClient)
        {
            //network protocol hands have schecten however we need some handshaking of our own for establishing player numbers, etc.
            BinaryFormatter formatter = new BinaryFormatter();

            InitialNetworkData clientData = (InitialNetworkData)formatter.Deserialize(gClient.Client.GetStream());
            gClient.ClientName = clientData.PlayerName;

            clientData.PlayerId = gClient.ClientId;
#warning need to finish the notification of other players

            formatter.Serialize(gClient.Client.GetStream(), clientData);

            return true;
        }
    }
}
