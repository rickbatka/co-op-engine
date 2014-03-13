using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using co_op_engine.Components;

namespace co_op_engine.Networking
{
    /// <summary>
    /// Serializable command meant to pass functional information over the network
    /// </summary>
    [Serializable]
    public struct CommandObject
    {
        public int ClientId;
        public readonly int ObjectId;
        Action<GameObject, object> Command;
        public readonly object ParamsObject;

        public CommandObject(GameObject receiver, Action<GameObject, object> command, object paramsObject, int clientId)
        {
            ObjectId = receiver.ID;
            Command = command;
            ParamsObject = paramsObject;
            ClientId = clientId;
        }

        /// <summary>
        /// privides wrapped execution of the command
        /// </summary>
        /// <param name="receiver">the gameobject on which the command will run</param>
        public void ExecuteCommand(GameObject receiver)
        {
            Command(receiver, ParamsObject);
        }
    }
}
