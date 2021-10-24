namespace Oliviann.Windows.Input
{
    #region Usings

    using System;

    #endregion

    /// <summary>
    /// Represents a class for relaying a command in WPF when implementing MVVM.
    /// </summary>
    /// <typeparam name="T">The type of input parameter.</typeparam>
    /// <seealso cref="System.Windows.Input.ICommand" />
    public class RelayCommand<T> : RelayCommand
    {
        #region Fields

        /// <summary>
        /// The target method to be executed.
        /// </summary>
        private readonly Action<T> targetMethod;

        /// <summary>
        /// The target method that determines the target method can be executed.
        /// </summary>
        private readonly Func<T, bool> canTargetMethod;

        #endregion

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand{T}"/>
        /// class.
        /// </summary>
        /// <param name="executeMethod">The method to be executed.</param>
        public RelayCommand(Action<T> executeMethod) : this(executeMethod, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand{T}"/>
        /// class.
        /// </summary>
        /// <param name="executeMethod">The method to be executed.</param>
        /// <param name="canExecuteMethod">The function that can determine the
        /// method can be executed.</param>
        public RelayCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod) : base(null, null)
        {
            this.targetMethod = executeMethod;
            this.canTargetMethod = canExecuteMethod;
        }

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
        public new bool CanExecute(object parameter)
        {
            if (this.canTargetMethod != null && parameter is T param)
            {
                return this.canTargetMethod(param);
            }

            return this.targetMethod != null;
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command
        /// does not require data to be passed, this object can be set to null.
        /// </param>
        public new void Execute(object parameter)
        {
            if (this.targetMethod != null && parameter is T param)
            {
                this.targetMethod?.Invoke(param);
            }
        }

        #endregion
    }
}