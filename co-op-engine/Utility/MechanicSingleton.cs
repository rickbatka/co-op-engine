using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using co_op_engine.Networking;

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
        public int MaxPlayers = 1;

        private MechanicSingleton()
        {
            rand = new Random(DateTime.Now.Millisecond);
        }

        static public void SetupFromNetwork(InitialNetworkData data)
        {
            Instance.MaxPlayers = data.MaxPlayers;
            Instance.PlayerId = data.PlayerId;
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
    }
}
