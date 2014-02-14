using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Utility
{
    class MechanicSingleton
    {
        private static MechanicSingleton _instance;
        public static MechanicSingleton Instance
        {
            get { return _instance ?? (new MechanicSingleton()); }
        }

        public Random rand;

        private MechanicSingleton()
        {
            rand = new Random(DateTime.Now.Millisecond);
        }
    }
}
