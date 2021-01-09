using System;
using System.Windows.Input;

namespace MotoiCal.Utilities.Commands
{
    public class SyncCommand : ICommand
    {
        protected readonly Func<bool> canExecute;

        protected readonly Action execute;

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (this.canExecute != null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }

            remove
            {
                if (this.canExecute != null)
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }

        public SyncCommand(Action execute, Func<bool> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public SyncCommand(Action execute) : this(execute, null)
        {
        }

        public virtual bool CanExecute(object parameter)
        {
            return this.canExecute == null ? true : this.canExecute();
        }

        public virtual void Execute(object parameter)
        {
            this.execute();
        }
    }

    public class SyncCommand<T> : ICommand
    {
        protected readonly Func<T, bool> canExecute;

        protected readonly Action<T> execute;

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (this.canExecute != null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }

            remove
            {
                if (this.canExecute != null)
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }

        public SyncCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public SyncCommand(Action<T> execute) : this(execute, null)
        {
        }

        public virtual bool CanExecute(object parameter)
        {
            return this.canExecute == null ? true : this.canExecute((T)parameter);
        }

        public virtual void Execute(object parameter)
        {
            this.execute((T)parameter);
        }
    }
}
