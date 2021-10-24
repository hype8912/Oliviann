namespace Oliviann.Diagnostics
{
    #region Usings

    using System;
    using System.Diagnostics;

    #endregion Usings

    /// <summary>
    /// Represents a custom trace listener to call a delegate method when the
    /// trace event is fired. Basically converts a <see cref="TraceListener"/>
    /// into an <see cref="EventHandler"/>.
    /// </summary>
    public class DelegateTraceListener : TraceListener
    {
        #region Fields

        /// <summary>
        /// Place holder for the delegate event.
        /// </summary>
        private readonly Action<string> targetDelegate;

        #endregion Fields

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="DelegateTraceListener"/> class.
        /// </summary>
        /// <param name="target">The target method to be executed.</param>
        /// <param name="name">The name of the <see cref="TraceListener"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">target can not be null.
        /// </exception>
        public DelegateTraceListener(Action<string> target, string name = null) : base(name)
        {
            ADP.CheckArgumentNull(target, nameof(target));
            this.targetDelegate = target;
        }

        #endregion Constructor/Destructor

        #region Writers

        /// <summary>
        /// Executes the specified target method when the
        /// <see cref="Trace.Write(string)"/> method is called.
        /// </summary>
        /// <param name="message">A message to write.</param>
        /// <filterpriority>2</filterpriority>
        public override void Write(string message) => this.targetDelegate?.Invoke(message);

        /// <summary>
        /// Executes the specified target method when the
        /// <see cref="Trace.WriteLine(string)"/> method is called, followed by
        /// a line terminator.
        /// </summary>
        /// <param name="message">A message to write.</param>
        /// <filterpriority>2</filterpriority>
        public override void WriteLine(string message) => this.Write(message + Environment.NewLine);

        /// <summary>
        /// Writes a category name and a message to the listener you create when
        /// you implement the <see cref="T:System.Diagnostics.TraceListener"/>
        /// class.
        /// </summary>
        /// <param name="message">A message to write.</param>
        /// <param name="category">A category name used to organize the output.
        /// </param>
        public override void Write(string message, string category)
        {
            if (!string.Equals(this.Name, category))
            {
                return;
            }

            this.targetDelegate?.Invoke(message);
        }

        /// <summary>
        /// Writes a category name and a message to the listener you create when
        /// you implement the <see cref="T:System.Diagnostics.TraceListener"/>
        /// class, followed by a line terminator.
        /// </summary>
        /// <param name="message">A message to write.</param>
        /// <param name="category">A category name used to organize the output.
        /// </param>
        public override void WriteLine(string message, string category) => this.Write(message + Environment.NewLine, category);

        #endregion Writers
    }
}