namespace Oliviann.Web.UI.WebControls
{
    #region Usings

    using System;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    #endregion Usings

    /// <summary>
    /// Represents an ASP.Net <see cref="GridView"/> control with a second
    /// header row that spans the whole table.
    /// </summary>
    [ToolboxData(@"<{0}:TwoHeaderGridView runat=""server""></{0}:TwoHeaderGridView>")]
    public class TwoHeaderGridView : GridView
    {
        #region Fields

        /// <summary>
        /// Variable for determining if the header row has been created in the
        /// table for the binding.
        /// </summary>
        private bool isHeaderRowCreated;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the text contents of the header.
        /// </summary>
        /// <value>
        /// The text contents of the header. The default value is
        /// <see cref="string.Empty"/>.
        /// </value>
        public string HeaderText
        {
            get
            {
                object value = this.ViewState["HeaderText"];
                if (value == null)
                {
                    return string.Empty;
                }

                return (string)value;
            }

            set
            {
                this.ViewState["HeaderText"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the Cascading Style Sheet (CSS) class rendered by the
        /// Web server control on the client.
        /// </summary>
        /// <value>
        /// The CSS class rendered by the Web server control on the client.
        /// </value>
        public string HeaderCssClass { get; set; }

        /// <summary>
        /// Gets the inner table <see cref="GridView"/>.
        /// </summary>
        protected Table InnerTable
        {
            get
            {
                if (this.HasControls())
                {
                    return (Table)this.Controls[0];
                }

                return null;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.DataBinding"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that
        /// contains the event data.</param>
        protected override void OnDataBinding(EventArgs e)
        {
            base.OnDataBinding(e);
            this.isHeaderRowCreated = false;
        }

        /// <summary>
        /// Raises the
        /// <see cref="E:System.Web.UI.WebControls.GridView.RowCreated"/> event.
        /// </summary>
        /// <param name="e">A
        /// <see cref="T:System.Web.UI.WebControls.GridViewRowEventArgs"/> that
        /// contains event data.</param>
        protected override void OnRowCreated(GridViewRowEventArgs e)
        {
            if (!this.isHeaderRowCreated)
            {
                this.CreateSecondHeader();
            }

            base.OnRowCreated(e);
        }

        /// <summary>
        /// Creates the second header row and adds it to the
        /// <see cref="InnerTable"/>.
        /// </summary>
        private void CreateSecondHeader()
        {
            if (this.InnerTable == null)
            {
                return;
            }

            // Sets the class style for the whole row (tr) object.
            var row = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal);
            if (this.HeaderCssClass != null)
            {
                row.CssClass = this.HeaderCssClass;
            }

            // Since a grid view doesn't properly register the column count we
            // try calling the first row of the table and see how many cells the
            // row actually contains. If no rows exist, we just call the normal
            // gridview columns count at that point. This does not take into
            // account that some columns may be set to not be visible.
            int columnSpan = this.InnerTable.Rows.Count > 0 ? this.InnerTable.Rows[0].Cells.Count : this.Columns.Count;
            var th = new TableHeaderCell
                         {
                             ColumnSpan = columnSpan,
                             HorizontalAlign = HorizontalAlign.Center,
                             Text = HttpUtility.HtmlEncode(this.HeaderText),
                         };

            row.Cells.Add(th);
            this.InnerTable.Rows.AddAt(0, row);
            this.isHeaderRowCreated = true;
        }

        #endregion Methods
    }
}