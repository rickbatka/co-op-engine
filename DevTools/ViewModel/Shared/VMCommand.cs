using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DevTools.ViewModel.Shared
{
    internal class VMCommand : ICommand
    {
        private Action<object> CommandAction;

        public VMCommand(Action<object> action)
        {
            CommandAction = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            CommandAction(parameter);
        }
    }
}
