namespace Oliviann.Windows.Controls
{
    #region Usings

    using System.Windows.Controls;
    using System.Windows.Data;

    #endregion

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// TextBox form controls.
    /// </summary>
    public static class TextBoxExtensions
    {
        /// <summary>
        /// Sets the specified text box binding to the specified value using two
        /// way binding.
        /// </summary>
        /// <param name="textBox">The text box control to set the binding.
        /// </param>
        /// <param name="bindingValue">The value to be bound to the Text
        /// property.</param>
        public static void SetTextBindingTwoWay(this TextBox textBox, string bindingValue) =>
            textBox?.SetBinding(TextBox.TextProperty, new Binding(bindingValue) { Mode = BindingMode.TwoWay });

        /// <summary>
        /// Sets the specified text box binding to the specified value using one
        /// way to source binding.
        /// </summary>
        /// <param name="textBox">The text box control to set the binding.
        /// </param>
        /// <param name="bindingValue">The value to be bound to the Text
        /// property.</param>
        public static void SetTextBindingOneWayToSource(this TextBox textBox, string bindingValue)
        {
            if (textBox != null)
            {
                string currentValue = textBox.Text;
                textBox.SetBinding(TextBox.TextProperty, new Binding(bindingValue) { Mode = BindingMode.OneWayToSource });
                textBox.Text = currentValue;
            }
        }
    }
}