﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using co_op_engine.Components;

namespace co_op_engine.Networking
{
    /// <summary>
    /// ONLY FEED THIS STATIC METHODS
    /// </summary>
    struct CommandObject
    {
        public readonly int ObjectId;
        Action<GameObject, object> Command;
        public readonly object ParamsObject;

        public CommandObject(GameObject receiver, Action<GameObject, object> command, object paramsObject)
        {
            ObjectId = receiver.ID;
            Command = command;
            ParamsObject = paramsObject;
        }

        public void ExecuteCommand(GameObject receiver)
        {
            Command(receiver, ParamsObject);
        }
    }
}