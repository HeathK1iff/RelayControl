using System;
using System.Windows.Input;

namespace MqttDevices.Utils
{
    public class GenericCommand : ICommand, ICommandEvents
    {
        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public event Action OnSuccess;
        public event Action<Exception> OnError;

        private readonly Action<object> _execute;
        private Func<object, bool> _canExecute;
        private IModelView _modelView;

        public GenericCommand(IModelView modelView, Action<object> execute, Func<object, bool> canExecute)
        {
            _modelView = modelView;
            _execute = execute;
            _canExecute = canExecute;
        }

        public GenericCommand(Action<object> execute, Func<object, bool> canExecute) : this(null, execute, canExecute) { }

        public GenericCommand(IModelView modelView) : this(modelView, null, null) { }

        public GenericCommand() : this(null, null, null) { }

        public IModelView ModelView { get => _modelView; }

        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke(parameter) ?? true;
        }

        public void Execute(object parameter)
        {
            try
            {
                DoExecute(parameter);
                OnSuccess?.Invoke();
            } catch (Exception e)
            {
                OnError?.Invoke(e);
            }
        }

        protected virtual void DoExecute(object parameter)
        {
            _execute?.Invoke(parameter);
        }
    }
}
