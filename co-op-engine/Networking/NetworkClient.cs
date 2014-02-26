using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using co_op_engine.Collections;


namespace co_op_engine.Networking
{
    class NetworkClient
    {
		//yes I realize the duplicate logic, I'll come in here and
		//  refactor that at some point, but I'm stuck using Notepad++
		//  right now....

#warning refactor this rediculous amount of duplicate logic.
		const int PORT = 22001;
		
		public event EventHandler
		
		private ThreadSafeBuffer<CommandObject> inputBuffer;
		public ThreadSafeBuffer<CommandObject> Input
		{
			get { return inputBuffer; }
		}
		
        private ThreadSafeBuffer<CommandObject> outputBuffer;
		public ThreadSafeBuffer<CommandObject> Output
		{
			get { return outputBuffer; }
		}
		
		private Thread recvThread;
		private Thread sendThread;
		
		public void ConnectToGame()
        {
        }

        public void QueueCommand(CommandObject command)
        {
        }

        public List<CommandObject> GetPendingCommands()
        {
            throw new NotImplementedException();
        }
    }
}