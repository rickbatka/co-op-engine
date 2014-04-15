using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using co_op_engine.Networking;
using Microsoft.Xna.Framework;

namespace co_op_engine.Utility
{
    class MechanicSingleton
    {
        private static MechanicSingleton _instance;
        public static MechanicSingleton Instance
        {
            get { return _instance ?? (_instance = new MechanicSingleton()); }
        }

        public Random rand;
        
        private int objectIdCounter = 0; //maybe long?
        private object objectIdCounterLocker = new object();
        public int PlayerId = 0;
        private int PlayerIdIndex = 0;
        public int MaxPlayers = 8;
        public string PlayerName;
        public string[] PlayerNames;

        private MechanicSingleton()
        {
            rand = new Random(DateTime.Now.Millisecond);
        }

        /// <summary>
        /// Sets up a client with server data
        /// </summary>
        /// <param name="data">incoming server data</param>
        static public void SetupAsClient(InitialNetworkData data)
        {
            Instance.MaxPlayers = data.MaxPlayers;
            Instance.PlayerId = data.PlayerId;

            Instance.PlayerNames = data.PlayerNames;
            Instance.PlayerNames[Instance.PlayerId] = data.PlayerName;
        }

        /// <summary>
        /// Adds to a server's data when a client connects
        /// </summary>
        /// <param name="data">incoming client data</param>
        static public InitialNetworkData SetupClientData(InitialNetworkData data)
        {
            ++Instance.PlayerIdIndex;
            Instance.PlayerNames[Instance.PlayerIdIndex] = data.PlayerName;

            return new InitialNetworkData()
            {
                MaxPlayers = Instance.MaxPlayers,
                PlayerId = Instance.PlayerIdIndex,
                PlayerNames = Instance.PlayerNames,
            };
        }

        static public void InitializeWithSettings(string playername, int maxPlayers)
        {
            Instance.PlayerName = playername;
            Instance.MaxPlayers = maxPlayers;
            Instance.PlayerNames = new string[maxPlayers];
            Instance.PlayerNames[0] = playername;
            Instance.PlayerIdIndex = 0;
        }

        public int GetNextObjectCountValue()
        {
            int threadsafeTemp = 0;
            lock (objectIdCounterLocker)
            {
                threadsafeTemp = (objectIdCounter * MaxPlayers) + PlayerId;
                ++objectIdCounter;
            }
            return threadsafeTemp;
        }

        public Vector2 RandomNormalizedVector()
        {
            int newX = 1;
            int newY = 1;
            if(rand.Next(0,9) < 5)
            {
                newX = -1;
            }

            if (rand.Next(0,9) < 5)
            {
                newY = -1;
            }
            return new Vector2(newX, newY);
        }

    }
}
