namespace Oliviann.Tests.TestObjects
{
    #region Usings

    using Oliviann.ComponentModel;

    #endregion Usings

    /// <summary>
    /// Represents a
    /// </summary>
    public class MocPropChangeClass : NotifyPropertyChange
    {
        #region Fields

        private string _propString;

        private string _propString2;

        private int _propInt;

        private long _propLong;

        #endregion Fields

        #region Properties

        public string PropString
        {
            get
            {
                return this._propString;
            }

            set
            {
                if (this._propString == value)
                {
                    return;
                }

                string oldValue = this._propString;
                this._propString = value;
                this.OnPropertyChange(oldValue, this._propString);
            }
        }

        public int PropInt
        {
            get
            {
                return this._propInt;
            }

            set
            {
                if (this._propInt == value)
                {
                    return;
                }

                int oldValue = this._propInt;
                this._propInt = value;
                this.OnPropertyChange(this, new PropertyChangeEventArgs(nameof(this.PropInt), oldValue, this._propInt));
            }
        }

        public string PropString2
        {
            get { return this._propString2; }
            set { this.SetProperty(ref this._propString2, value); }
        }

        public long PropLong
        {
            get { return this._propLong; }
            set { this.SetProperty(ref this._propLong, value); }
        }

        #endregion Properties
    }
}