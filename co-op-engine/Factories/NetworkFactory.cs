using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using co_op_engine.Components;
using co_op_engine.Components.Brains;
using co_op_engine.Components.Brains.TowerBrains;
using co_op_engine.GameStates;
using co_op_engine.Networking;
using co_op_engine.Networking.Commands;

namespace co_op_engine.Factories
{
    /// <summary>
    /// sadly this thing is gonna have to build everything, 
    /// probably gonna leverage the other factories in doing
    /// so
    /// </summary>
    public class NetworkFactory
    {
        private static NetworkFactory instance;
        public static NetworkFactory Instance { get { return instance; } }

        private GamePlay GameRef;
        private NetworkBase netRef;

        public static void Initialize(GameStates.GamePlay gamePlay, NetworkBase netref)
        {
            instance = new NetworkFactory();
            instance.GameRef = gamePlay;
            instance.netRef = netref;
        }

        public static GameObject BuildFromNetwork(CreateParameters parameters)
        {
            switch (parameters.ConstructionId)
            {
                case "Player":
                    {
                        return PlayerFactory.Instance.GetNetworkPlayer(parameters.ID);
                    }
                case "Tower":
                    {
                        return TowerFactory.Instance.GetDoNothingTower(true, parameters.ID);
                    }
                default:
                    {
                        throw new Exception("The Object Hasn't Been Set up to be created throug hthe network yet");
                    }
            }
        }
    }
}
