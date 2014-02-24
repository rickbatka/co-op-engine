using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using co_op_engine.Collections;

namespace co_op_engine.Networking
{
    class NetworkServer
    {
        const int PORT = 22001;

        public event EventHandler OnClientConnect;
        public event EventHandler OnClientDisconnect;
        public event EventHandler OnNetworkError;

        private ThreadSafeBuffer<CommandObject> inputBuffer;
        private ThreadSafeBuffer<CommandObject> outputBuffer;
        private Thread listenThread;
        private List<Thread> clientThreads;
        private List<TcpClient> clients;
        private TcpListener listener;

        public NetworkServer()
        {
            inputBuffer = new ThreadSafeBuffer<CommandObject>();
            outputBuffer = new ThreadSafeBuffer<CommandObject>();
            clientThreads = new List<Thread>();
            clients = new List<TcpClient>();

            listenThread = new Thread(new ThreadStart(ListenLoop));
            listenThread.IsBackground = true;
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
        }

        public void QueueCommand(CommandObject command)
        {
        }

        public List<CommandObject> GetPendingCommands()
        {
            throw new NotImplementedException();
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

                    clients.Add(inClient);
                    Thread inClientRecvThread = new Thread(new ParameterizedThreadStart(ClientRecvLoop));
                    inClientRecvThread.IsBackground = true;
                    clientThreads.Add(inClientRecvThread);

                    inClientRecvThread.Start(inClient);
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

        public void ClientRecvLoop(object clientObj)
        {
            NetworkClient client = (NetworkClient)clientObj;
            //network protocol hands have schecten however we need some handshaking of our own for establishing player numbers, etc.
            
            //need to do locked check against player number increment
            //send client a struct dictating player number and other date

            //receive client information and set a client object
            //build a command object for adding a network player with this info

            //listen for chatter
            //send chatter to output
        }
    }
}
