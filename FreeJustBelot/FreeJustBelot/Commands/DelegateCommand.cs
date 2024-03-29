﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FreeJustBelot.Commands
{
    public delegate void ExecuteCommandDelegate<T>(T parameter);

    public delegate bool CanExecuteCommandDelegate<T>(T parameter);

    public class DelegateCommand<T> : ICommand
    {
        private ExecuteCommandDelegate<T> execute;
        private CanExecuteCommandDelegate<T> canExecute;

        public DelegateCommand(ExecuteCommandDelegate<T> execute) : this(execute, null)
        {
        }

        public DelegateCommand(ExecuteCommandDelegate<T> execute, CanExecuteCommandDelegate<T> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (this.canExecute == null)
            {
                return true;
            }

            if (parameter == null)
            {
                return false;
            }
            
            return this.canExecute((T)parameter);
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            this.execute((T)parameter);
        }
    }
}