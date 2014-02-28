using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using co_op_engine.Networking;
using System.Xml.Serialization;

namespace DevTools.Model
{
    class GameClientWrapper
    {
        NetworkClient client;
        NetworkServer server;

        public GameClientWrapper()
        {
        }

        public void SpinUpServer()
        { }

        public void SpinUpClient(string ip = "")
        {
            if (ip == string.Empty)
            {
                ip = "127.0.0.1";
            }
        }
    }
}
