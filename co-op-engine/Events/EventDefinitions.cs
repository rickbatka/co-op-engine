using co_op_engine.Components;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using System;

namespace co_op_engine.Events
{
    public class ActorStateChangedEventArgs : EventArgs
    {
        public ActorState OldState;
        public ActorState NewState;
    }

    public class ActorDirectionChangedEventArgs : EventArgs
    {
        public int OldDirection;
        public int NewDirection;
    }

    public class CoordEventArgs : EventArgs
    {
        public Vector2 Coords;
    }
}
