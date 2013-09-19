using System;
using System.Windows.Input;

namespace ILB.Wpf
{
    public class DelegateCommand : ICommand 
    {
        private readonly Action _func;

        public DelegateCommand(Action func)
        {
            _func = func;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _func();
        }

        public event EventHandler CanExecuteChanged;
    }
}