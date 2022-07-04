using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChatApp.MVVM.Core
{
    public class RelayCommand : ICommand
    {
        private Action<object> execute;
        private Func<object,bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Func<object,bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parametar)
        {
            return this.canExecute == null || this.canExecute(parametar);
        }

        public void Execute(object parametar)
        {
            this.execute(parametar);
        }
    }
}
