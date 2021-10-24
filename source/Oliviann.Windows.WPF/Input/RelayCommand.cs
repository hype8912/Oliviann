namespace Oliviann.Windows.Input
{
    #region Usings

    using System;
    using System.Windows.Input;

    #endregion

    /// <summary>
    /// Represents a class for relaying a command in WPF when implementing MVVM.
    /// </summary>
    public class RelayCommand : ICommand
    {
        #region Fields

        /// <summary>
        /// The target method to be executed.
        /// </summary>
        private readonly Action targetMethod;

        /// <summary>
        /// The target method that determines the target method can be executed.
        /// </summary>
        private readonly Func<bool> canTargetMethod;

        #endregion

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="executeMethod">The method to be executed.</param>
        public RelayCommand(Action executeMethod) : this(executeMethod, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="executeMethod">The method to be executed.</param>
        /// <param name="canExecuteMethod">The function that can determine the
        /// method can be executed.</param>
        public RelayCommand(Action executeMethod, Func<bool> canExecuteMethod)
        {
            this.targetMethod = executeMethod;
            this.canTargetMethod = canExecuteMethod;
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command
        /// should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        #endregion

        #region Methods

        /// <summary>
        /// Defines the method that determines whether the command can execute
        /// in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command
        /// does not require data to be passed, this object can be set to null.
        /// </param>
        /// <returns>True if this command can be executed; otherwise, false.
        /// </returns>
        public virtual bool CanExecute(object parameter)
        {
            if (this.canTargetMethod != null)
            {
                return this.canTargetMethod();
            }

            return this.targetMethod != null;
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command
        /// does not require data to be passed, this object can be set to null.
        /// </param>
        public virtual void Execute(object parameter) => this.targetMethod?.Invoke();

        /// <summary>
        /// Raises the can execute changed event.
        /// </summary>
        public void RaiseCanExecuteChanged() => this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        #endregion
    }
}