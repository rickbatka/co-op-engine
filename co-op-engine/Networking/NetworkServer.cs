﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using co_op_engine.Collections;
using co_op_engine.Networking.Commands;
using co_op_engine.Utility;
using co_op_engine.World.Level;

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

        private ThreadSafeBuffer<NetworkCommandObject> inputBuffer;
        public override ThreadSafeBuffer<NetworkCommandObject> Input
        {
            get { return inputBuffer; }
        }

        private ThreadSafeBuffer<NetworkCommandObject> outputBuffer;
        public override ThreadSafeBuffer<NetworkCommandObject> Output
        {
            get { return outputBuffer; }
        }

        private Thread listenThread;
        private Thread sendThread;
        private List<Thread> clientThreads;
        private List<GameClient> clients;
        private TcpListener listener;
        private int playerIndex;

        public override int ClientId
        {
            get { return 0; }
        }

        public NetworkServer()
        {
            inputBuffer = new ThreadSafeBuffer<NetworkCommandObject>();
            outputBuffer = new ThreadSafeBuffer<NetworkCommandObject>();
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
                sendThread.Start();
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

            try
            {
                sendThread.Abort();
            }
            catch
            { }

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
                        ++base.SentCount;
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
                    NetworkCommandObject command = (NetworkCommandObject)formatter.Deserialize(netStream);
                    ++base.RecvCount;
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

        private void EchoAllOthers(NetworkCommandObject command, BinaryFormatter formatter = null)
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
            var stream = gClient.Client.GetStream();

            InitialNetworkData clientData = (InitialNetworkData)formatter.Deserialize(stream);

            var response = MechanicSingleton.SetupClientData(clientData);

            formatter.Serialize(stream, response);

            foreach (var obj in worldRef.GetWorldForNetwork())
            {

                formatter.Serialize(stream,
                    new NetworkCommandObject()
                    {
                        ClientId = gClient.ClientId,
                        Command = obj,
                    });

            }

            return true;
        }
    }
}
