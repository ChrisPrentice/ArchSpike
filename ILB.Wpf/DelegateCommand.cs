using System;
using System.Windows.Input;

namespace ILB.Wpf
{
    public class DelegateCommand : ICommand 
    {
        private readonly Action func;

        public DelegateCommand(Action func)
        {
            this.func = func;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            func();
        }

        public event EventHandler CanExecuteChanged;
    }
}