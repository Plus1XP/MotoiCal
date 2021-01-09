using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MotoiCal.Utilites.Commands
{
    public class SynchronousRelayCommand : ICommand
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

        public SynchronousRelayCommand(Action execute, Func<bool> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public SynchronousRelayCommand(Action execute)
            : this(execute, null)
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

    public class SynchronousRelayCommand<T> : ICommand
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

        public SynchronousRelayCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public SynchronousRelayCommand(Action<T> execute)
            : this(execute, null)
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

    public class AsynchronousRelayCommand : SynchronousRelayCommand
    {
        private bool isExecuting = false;

        public event EventHandler Started;

        public event EventHandler Ended;

        public bool IsExecuting
        {
            get { return this.isExecuting; }
        }

        public AsynchronousRelayCommand(Action execute, Func<bool> canExecute)
            : base(execute, canExecute)
        {
        }

        public AsynchronousRelayCommand(Action execute)
            : base(execute)
        {
        }

        public override bool CanExecute(object parameter)
        {
            return ((base.CanExecute(parameter)) && (!this.isExecuting));
        }

        public override void Execute(object parameter)
        {
            try
            {
                this.isExecuting = true;
                if (this.Started != null)
                {
                    this.Started(this, EventArgs.Empty);
                }

                Task task = Task.Factory.StartNew(() =>
                {
                    this.execute();
                });
                task.ContinueWith(t =>
                {
                    this.OnRunWorkerCompleted(EventArgs.Empty);
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
            catch (Exception ex)
            {
                this.OnRunWorkerCompleted(new RunWorkerCompletedEventArgs(null, ex, true));
            }
        }

        private void OnRunWorkerCompleted(EventArgs e)
        {
            this.isExecuting = false;
            if (this.Ended != null)
            {
                this.Ended(this, e);
            }
        }
    }
}
