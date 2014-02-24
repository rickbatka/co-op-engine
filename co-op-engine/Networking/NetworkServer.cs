using System;
using System.Collections.Generic;
using System.Linq;
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
            //instantiate listener
            //start listen loop
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
            //listen
            //add to list
            //send to clientrecv loop
        }

        public void ClientRecvLoop(object clientObj)
        {
            NetworkClient client = (NetworkClient)clientObj;

            //listen for chatter
            //send chatter to output
        }
    }
}
