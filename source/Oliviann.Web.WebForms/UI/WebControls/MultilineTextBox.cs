namespace Oliviann.Web.UI.WebControls
{
    #region Usings

    using System;
    using System.ComponentModel;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    #endregion Usings

    /// <summary>
    /// Fixes _bug in asp.net with multi line text box not enforcing max length
    /// value.
    /// </summary>
    [DefaultProperty(@"Text")]
    [ToolboxData(@"<{0}:MultilineTextBox runat=server></{0}:MultilineTextBox>")]
    public class MultilineTextBox : TextBox
    {
        #region Fields

        /// <summary>
        /// Name of max length attribute.
        /// </summary>
        private const string MaxLengthAttributeName = @"exMaxLen";

        #endregion Fields

        #region Overrides

        /// <summary>
        /// Registers client script for generating postback events prior to
        /// rendering on the client, if
        /// <see cref="P:System.Web.UI.WebControls.TextBox.AutoPostBack"/> is
        /// true.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains
        /// the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            // Ensure default work takes place.
            base.OnPreRender(e);

            // Configure TextArea support. The System.Web.UI.WebControls.TextBox does not
            // honor the "MaxLength" attribute when the "Multiline" textmode is selected.
            if (this.TextMode != TextBoxMode.MultiLine || this.MaxLength <= 0)
            {
                return;
            }

            // If we haven't already, include the supporting script that limits the content of textareas.
            this.Page.ClientScript.RegisterClientScriptResource(
                typeof(MultilineTextBox),
                HttpContext.Current.IsDebuggingEnabled ? @"Oliviann.Web.tb_scripts.debug.js" : @"Oliviann.Web.tb_scripts.js");

            // Add an expando attribute to the rendered control which sets
            // its maximum length (using the MaxLength Attribute)
            // Using RegisterExpandoAttribute maintains XHTML compliance on
            // the rendered page.
            //
            // Where there is a ScriptManager on the parent page, use it
            // to register the attribute - to ensure the control
            // works in partial updates (like an AJAX UpdatePanel)
            if (!ScriptManager.GetCurrent(this.Page).IsNull())
            {
                ScriptManager.RegisterExpandoAttribute(
                                                       this,
                                                       this.ClientID,
                                                       MaxLengthAttributeName,
                                                       this.MaxLength.ToString(),
                                                       true);
            }
            else
            {
                this.Page.ClientScript.RegisterExpandoAttribute(this.ClientID, MaxLengthAttributeName, this.MaxLength.ToString());
            }

            this.Add_Attributes();
        }

        #endregion Overrides

        /// <summary>
        /// Binds the attributes for <c>onkeydown</c>, <c>oninput</c>, and
        /// <c>onpaste</c> events to script that we'll inject in the parent
        /// page.
        /// </summary>
        private void Add_Attributes()
        {
            this.Attributes.Add("onkeydown", "javascript:return LimitInput(this, event);");
            this.Attributes.Add("oninput", "javascript:return LimitInput(this, event);");
            this.Attributes.Add("onpaste", "javascript:return LimitPaste(this, event);");
        }
    }
}