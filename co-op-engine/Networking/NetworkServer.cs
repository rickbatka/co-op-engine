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
    class NetworkServer
    {
        const int PORT = 22001;

        public event EventHandler OnClientConnect;
        public event EventHandler OnClientDisconnect;
        public event EventHandler OnNetworkError;

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
		
        private Thread listenThread;
		private Thread sendThread;
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
			
			sendThread = new Thread(new ThreadStart(SendLoop));
			sendThread.IsBackground = true;
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
                    client.Close();
                }
                catch
                { }
            }

            clients.Clear();
        }

        public void QueueCommand(CommandObject command)
        {
			inputBuffer.AddToList(command);
        }

        public List<CommandObject> GetPendingCommands()
        {
            return outputBuffer.RetrieveData();
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

		private void SendLoop()
		{
			while(true)
			{
				var queued = inputBuffer.RetrieveData();
				if(queued.Count() == 0)
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
					Foreach(var command in queued)
					{
						EchoAllOthers(command);
					}
				}
			}
		}
		
        public void ClientRecvLoop(object clientObj)
        {
            TcpClient client = (TcpClient)clientObj;
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                NetworkStream netStream = client.GetStream();
                //network protocol hands have schecten however we need some handshaking of our own for establishing player numbers, etc.

                //need to do locked check against player number increment
                //send client a struct dictating player number and other date

                //receive client information and set a client object
                //build a command object for adding a network player with this info

                while (true)
                {
                    //blocks here
                    CommandObject command = (CommandObject)formatter.Deserialize(netStream);

                    //send chatter to output
                    EchoAllOthers(client, command, formatter);

                    outputBuffer.AddToList(command);
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
                    client.Close();

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
			if( formatter == null) formatter = new BinaryFormatter();
            for (int i = 0; i < clients.Count(); ++i)
            {
				formatter.Serialize(client[i].GetStream(), command);					
            }
        }
    }
}
