using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using co_op_engine.Components;
using co_op_engine.Components.Brains;
using co_op_engine.Components.Brains.TowerBrains;
using co_op_engine.GameStates;
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
        public static NetworkFactory Instance { get { return instance; }}

        private GamePlay GameRef;

        public static void Initialize(GameStates.GamePlay gamePlay)
        {
            instance = new NetworkFactory();
            instance.GameRef = gamePlay;
        }

        public static GameObject BuildFromNetwork(CreateParameters parameters)
        {
            if (parameters.Brain == typeof(PlayerBrain))
            {
                //hack to speed things up
                return PlayerFactory.Instance.GetNetworkPlayer();
            }
            else if (parameters.Brain == typeof(BasicTowerBrain))
            {
                return TowerFactory.Instance.GetDoNothingTower();
            }
            throw new NotImplementedException("other objects haven't been dealt with across network yet");
        }
    }
}
