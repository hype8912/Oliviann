namespace Oliviann.ComponentModel
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    #endregion Usings

    /// <summary>
    /// Represents a base class for notifying that a property has changed the
    /// includes the old value and the new value.
    /// </summary>
    [Serializable]
    public abstract class NotifyPropertyChange : INotifyPropertyChange
    {
        #region Events

        /// <summary>
        /// Occurs when a property value changes. Event arguments returns
        /// property name, old value, and new value.
        /// </summary>
        public event EventHandler<PropertyChangeEventArgs> PropertyChange;

        #endregion Events

        #region Change Events

        /// <summary>
        /// Removes all the event subscribers.
        /// </summary>
        public void ClearEvents() => this.PropertyChange = null;

        /// <summary>
        /// Called to safely raise the changed event of the property.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="data">A
        /// <see cref="PropertyChangeEventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnPropertyChange(object sender, PropertyChangeEventArgs data)
        {
            ////System.Diagnostics.Debug.WriteLine("Raising Event On Property Change: " + data.PropertyName);
            this.PropertyChange?.RaiseEvent(sender, data);
        }

        /// <summary>
        /// Called to safely raise the changed event of the property.
        /// </summary>
        /// <param name="oldValue">The old property value.</param>
        /// <param name="newValue">The new property value.</param>
        /// <param name="propertyName">The name of the property that changed.
        /// </param>
        protected void OnPropertyChange(object oldValue, object newValue, [CallerMemberName] string propertyName = "") =>
            this.OnPropertyChange(this, new PropertyChangeEventArgs(propertyName, oldValue, newValue));

        /// <summary>
        /// Determines if the property value is being changed, sets the property
        /// value to the new value if different, and then raises the property
        /// change event.
        /// </summary>
        /// <typeparam name="T">The type of value being set</typeparam>
        /// <param name="currentValue">A reference to the current value of the
        /// property.</param>
        /// <param name="newValue">The new value to be set to the property if
        /// different.</param>
        /// <param name="propertyName">The name of the property.</param>
        protected void SetProperty<T>(ref T currentValue, T newValue, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(currentValue, newValue))
            {
                return;
            }

            T oldValue = currentValue;
            currentValue = newValue;
            this.OnPropertyChange(oldValue, newValue, propertyName);
        }

        #endregion Change Events
    }
}