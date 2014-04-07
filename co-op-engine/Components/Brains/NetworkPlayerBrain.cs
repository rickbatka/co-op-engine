using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Brains
{
    class NetworkPlayerBrain : BrainBase
    {
        //has a networking command queue


        public NetworkPlayerBrain(GameObject player)
            : base(player)
        { }

        public override void ReceiveCommand(Networking.Commands.GameObjectCommand command)
        {
            PlayerBrain.PlayerBrainUpdateParams parms = (PlayerBrain.PlayerBrainUpdateParams)command.Parameters;

            owner.InputMovementVector = parms.InputMovementVector;
        }
    }
}
