namespace Oliviann.Data
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Data;

    #endregion Usings

    /// <summary>
    /// Represents a partial class to the main DbManager class for handling
    /// parameters.
    /// </summary>
    public sealed partial class DbManager
    {
        #region Fields

        /// <summary>
        /// Place holder for the actual parameters list.
        /// </summary>
        private IList<IDataParameter> _parameters;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets a copy of the parameters for the current instance. This only
        /// provides a copy of the internal parameters collection.
        /// </summary>
        /// <value>The command parameters.</value>
        public IList<IDataParameter> Parameters
        {
            get
            {
                var parms = new List<IDataParameter>();
                if (this._parameters != null)
                {
                    parms.AddRange(this._parameters);
                }

                return parms;
            }
        }

        #endregion Properties

        #region Parameters

        /// <summary>
        /// Adds the specified parameter to the parameter collection.
        /// </summary>
        /// <param name="parameter">The data parameter to be added.</param>
        public void AddParameter(IDataParameter parameter)
        {
            if (parameter == null)
            {
                return;
            }

            if (this._parameters == null)
            {
                this._parameters = new List<IDataParameter>();
            }

            this._parameters.Add(parameter);
        }

        /// <summary>
        /// Removes all <see cref="IDataParameter"/> objects from the parameter
        /// collection.
        /// </summary>
        public void ClearParameters()
        {
            this.Command?.Parameters?.Clear();
            this._parameters?.Clear();
        }

        /// <summary>
        /// Gets the value of the specified parameter.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns>An object that is the value of the parameter. The default
        /// value is <c>null</c>.</returns>
        public object GetParameterValue(string parameterName) => ((IDataParameter)this.Command.Parameters[parameterName]).Value;

        #endregion Parameters

        #region Parameter Helpers

        /// <summary>
        /// Attaches the parameters in the parameter list to the database
        /// command.
        /// </summary>
        private void AttachParameters()
        {
            foreach (IDataParameter parameter in this._parameters)
            {
                if (parameter.Direction == ParameterDirection.Input)
                {
                    if (parameter.Value == null)
                    {
                        parameter.Value = DBNull.Value;
                    }
                    else if (parameter.Value is DateTime date)
                    {
                        parameter.Value = Base.DbManagerFactory.GetDbDateTime(this.Provider, date);
                    }
                }

                this.Command.Parameters.Add(parameter);
            }
        }

        #endregion Parameter Helpers
    }
}