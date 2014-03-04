using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevTools.Model;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace DevTools.ViewModel
{
    class ConsoleViewModel : INotifyPropertyChanged
    {
        GameClientWrapper wrapper;

        public ConsoleViewModel()
        {
            wrapper = new GameClientWrapper(AddMessage);
            _messages = new ObservableCollection<string>();
            AddMessage("Network Console");
        }

        public void Connect()
        {
            wrapper.SpinUpClient();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaiseProperyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        private ObservableCollection<string> _messages;
        public ObservableCollection<string> Messages
        {
            get { return _messages; }
        }


        public void AddMessage(string message)
        {
            _messages.Add(message);
            RaiseProperyChanged("Messages");
        }

        internal void Disconnect()
        {
            wrapper.client.DisconnectFromGame();
        }

        internal void Host()
        {
        }

        internal void StopHost()
        {
        }

        internal void ExecuteCommand(string p)
        {
            AddMessage(p);
        }
    }
}
