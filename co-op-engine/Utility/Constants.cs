using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Utility
{
    public static class Constants
    {
        public static readonly int South = 0;
        public static readonly int West = 1;
        public static readonly int North = 2;
        public static readonly int East = 3;
    }
    public enum ActorState
    {
        Idle = 0,
        Walking = 1
    }
}
