using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using co_op_engine.Components;

namespace co_op_engine.Networking.Commands
{
    //placeholder option for networking ease
    public class NetCommander
    {
        private static NetCommander instance;

        public static int SentCount
        {
            get { return instance.NetReference.SentCount; }
        }

        public static int RecvCount
        {
            get { return instance.NetReference.RecvCount; }
        }

        private NetworkBase NetReference;


        private NetCommander(NetworkBase network)
        {
            NetReference = network;
        }

        public static void SendCommand(NetworkCommandObject command)
        {
            instance.NetReference.Input.Add(command);
        }

        public static void SendCommand(GameObjectCommand command)
        {
            NetworkCommandObject completeCommand = new NetworkCommandObject()
            {
                ClientId = instance.NetReference.ClientId,
                Command = command,
                CommandType = NetworkCommandType.GameObjectCommand,
            };

            instance.NetReference.Input.Add(completeCommand);
        }

        internal static void SetNetwork(NetworkBase network)
        {
            instance = new NetCommander(network);
        }

        internal static void RegisterWorldWithNetwork(World.Level.ObjectContainer container)
        {
            instance.NetReference.RegisterWorldWithNetwork(container);
        }

        public static List<NetworkCommandObject> RendPendingCommands()
        {
            return instance.NetReference.Output.Gather();
        }

        public static void CreatedObject(GameObject player)
        {
            var parms = new CreateParameters()
            {
                ConstructorId = player.ConstructionStamp,
                ID = player.ID,
                Position = player.Position
            };

            SendCommand(new GameObjectCommand()
            {
                CommandType = GameObjectCommandType.Create,
                Parameters = parms,
            });
        }
    }
}
