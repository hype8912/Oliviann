namespace Oliviann.ComponentModel
{
    #region Usings

    using System;

    #endregion

    /// <summary>
    /// Notifies clients that a property value has changed.
    /// </summary>
    public interface INotifyPropertyChange
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        event EventHandler<PropertyChangeEventArgs> PropertyChange;
    }
}