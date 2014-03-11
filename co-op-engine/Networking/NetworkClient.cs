using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using co_op_engine.Collections;


namespace co_op_engine.Networking
{
    public delegate void ConnectedToServerEventHandler(NetworkClient sender, InitialNetworkData data);

    public class NetworkClient
    {
        //yes I realize the duplicate logic, I'll come in here and
        //  refactor that at some point, but I'm stuck using Notepad++
        //  right now....

#warning refactor this rediculous amount of duplicate logic.
        const int PORT = 22001;

        public event EventHandler OnNetworkError;
        public event EventHandler DEBUGNETWORKTRAFFIC;
        public event ConnectedToServerEventHandler OnServerConnected;

        private ThreadSafeBuffer<CommandObject> inputBuffer;
        public ThreadSafeBuffer<CommandObject> Input
        {
            get { return inputBuffer; }
        }

        private ThreadSafeBuffer<CommandObject> outputBuffer;
        public ThreadSafeBuffer<CommandObject> Output
        {
            get { return outputBuffer; }
        }

        private Thread recvThread;
        private Thread sendThread;

        private GameClient thisClient;

        public NetworkClient()
        {
            inputBuffer = new ThreadSafeBuffer<CommandObject>();
            outputBuffer = new ThreadSafeBuffer<CommandObject>();

            recvThread = new Thread(new ThreadStart(RecvLoop));
            recvThread.IsBackground = true;

            sendThread = new Thread(new ThreadStart(SendLoop));
            sendThread.IsBackground = true;
        }

        public void ConnectToGame(string ip)
        {
            if (thisClient.Client != null)
            {
                return;
            }

            thisClient = new GameClient()
            {
                Client = new TcpClient(),
                ClientId = -1 //this is the pending handshake state
            };

            try
            {
                thisClient.Client.Connect(ip, PORT);

                var valid = InitialHandshake();

                if (!valid)
                {
                    DisconnectFromGame();
                    throw new Exception("handshaking failed, he probably had a knife up his sleeve");
                }

                recvThread.Start();
                sendThread.Start();
            }
            catch(Exception e)
            {
                if (OnNetworkError != null)
                {
                    OnNetworkError(e, null);
                }
            }
        }

        public void DisconnectFromGame()
        {
            try
            { thisClient.Client.Close(); }
            catch
            { }

            try
            { recvThread.Abort(); }
            catch
            { }

            try
            { sendThread.Abort(); }
            catch
            { }

            thisClient.Client = null;
            thisClient.ClientId = -1;
        }

        private void SendLoop()
        {
            var stream = thisClient.Client.GetStream();
            var formatter = new BinaryFormatter();

            while (true)
            {
                var queued = inputBuffer.Gather();
                if (queued.Count() == 0)
                {
                    Thread.Sleep(100);
                }
                else
                {
                    try
                    {
                        foreach (var command in queued)
                        {
                            formatter.Serialize(stream, command);
                        }
                    }
                    catch
                    { }
                }
            }
        }

        private void RecvLoop()
        {
            var stream = thisClient.Client.GetStream();
            var formatter = new BinaryFormatter();

#warning currently unsafe, need to work on it
            while (true)
            {
                //blocks here
                CommandObject command = (CommandObject)formatter.Deserialize(stream);

                //send chatter to output
                outputBuffer.Add(command);
            }
        }

        private bool InitialHandshake()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            var stream = thisClient.Client.GetStream();

            InitialNetworkData initialData = new InitialNetworkData()
            {
                MaxPlayers = -1,
                PlayerId = -1,
                PlayerName = "TestClient",
                PlayerNames = null
            };

            formatter.Serialize(stream,initialData);
            initialData = (InitialNetworkData)formatter.Deserialize(stream);

            if (OnServerConnected != null)
            {
                OnServerConnected(this, initialData);
            }
            return true;
        }

        private void DebugNetworkTraffic(object traffic)
        {
            if (DEBUGNETWORKTRAFFIC != null)
            {
                string trafficObjectType = traffic.GetType().ToString();
                string trafficValues = "";
                foreach (var item in traffic.GetType().GetProperties())
                {
                    trafficValues += item.GetType().ToString() + " ";
                }

                DEBUGNETWORKTRAFFIC(trafficObjectType + ": " + trafficValues, null);
            }
        }
    }
}