namespace Oliviann.IO
{
    #region Usings

    using System;
    using Oliviann.Properties;

    #endregion Usings

    /// <summary>
    /// Represents a file size. Calculation for Zettabyte and above have been
    /// removed because of the maximum limits in the .NET Framework.
    /// </summary>
    [Serializable]
    public struct FileSize : IComparable, IComparable<FileSize>, IEquatable<FileSize>, IEquatable<ulong>
    {
        #region Fields

        /// <summary>
        /// Represent the number of bytes in 1 kilobyte. This field is constant.
        /// </summary>
        public const ulong BytesPerKilobyte = 1024L;

        /// <summary>
        /// Represent the number of bytes in 1 megabyte. This field is constant.
        /// </summary>
        public const ulong BytesPerMegabyte = 1048576L;

        /// <summary>
        /// Represent the number of bytes in 1 gigabyte. This field is constant.
        /// </summary>
        public const ulong BytesPerGigabyte = 1073741824L;

        /// <summary>
        /// Represent the number of bytes in 1 terabyte. This field is constant.
        /// </summary>
        public const ulong BytesPerTerabyte = 1099511627776;

        /// <summary>
        /// Represent the number of bytes in 1 petabyte. This field is constant.
        /// </summary>
        public const ulong BytesPerPetabyte = 1125899906842624;

        /// <summary>
        /// Represent the number of bytes in 1 exabyte. This field is constant.
        /// </summary>
        public const ulong BytesPerExabyte = 1152921504606846976;

        /// <summary>
        /// The collection of file size suffixes in order.
        /// </summary>
        private static readonly string[] FileSizeSuffix = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };

        /// <summary>
        /// Represents the minimum <see cref="FileSize"/> value. This field is
        /// read-only.
        /// </summary>
        public static readonly FileSize MinValue = new FileSize(ulong.MinValue);

        /// <summary>
        /// Represents the maximum <see cref="FileSize"/> value. This field is
        /// read-only.
        /// </summary>
        public static readonly FileSize MaxValue = new FileSize(ulong.MaxValue);

        /// <summary>
        /// Represents the zero <see cref="FileSize"/> value. This field is
        /// read-only.
        /// </summary>
        public static readonly ulong Zero = 0L;

        #endregion Fields

        #region Constructors/Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSize" /> struct to
        /// the specified number of bytes.
        /// </summary>
        /// <param name="bytes">The size expressed in 8-bit units.</param>
        public FileSize(ulong bytes) => this.Bytes = bytes;

        #endregion Constructors/Destructors

        #region Properties

        /// <summary>
        /// Gets the number of bytes that represent the value of the current
        /// <see cref="FileSize"/> structure.
        /// </summary>
        /// <value>
        /// The number of bytes contained in this instance.
        /// </value>
        public ulong Bytes { get; }

        /// <summary>
        /// Gets the value of the current <see cref="FileSize"/> structure
        /// expressed in whole or fractional kilobytes.
        /// </summary>
        /// <value>
        /// The total number of kilobytes represented by this instance.
        /// </value>
        public double Kilobytes => this.ScaleUpFromBytes(1);

        /// <summary>
        /// Gets the value of the current <see cref="FileSize"/> structure
        /// expressed in whole or fractional megabytes.
        /// </summary>
        /// <value>
        /// The total number of megabytes represented by this instance.
        /// </value>
        public double Megabytes => this.ScaleUpFromBytes(2);

        /// <summary>
        /// Gets the value of the current <see cref="FileSize"/> structure
        /// expressed in whole or fractional gigabytes.
        /// </summary>
        /// <value>
        /// The total number of gigabytes represented by this instance.
        /// </value>
        public double Gigabytes => this.ScaleUpFromBytes(3);

        /// <summary>
        /// Gets the value of the current <see cref="FileSize"/> structure
        /// expressed in whole or fractional terabytes.
        /// </summary>
        /// <value>
        /// The total number of terabytes represented by this instance.
        /// </value>
        public double Terabytes => this.ScaleUpFromBytes(4);

        /// <summary>
        /// Gets the value of the current <see cref="FileSize"/> structure
        /// expressed in whole or fractional petabytes.
        /// </summary>
        /// <value>
        /// The total number of petabytes represented by this instance.
        /// </value>
        public double Petabytes => this.ScaleUpFromBytes(5);

        /// <summary>
        /// Gets the value of the current <see cref="FileSize"/> structure
        /// expressed in whole or fractional exaabytes.
        /// </summary>
        /// <value>
        /// The total number of exaabytes represented by this instance.
        /// </value>
        public double Exabytes => this.ScaleUpFromBytes(6);

        #endregion Properties

        #region Methods

        #region Static Methods

        /// <summary>
        /// Compares two <see cref="FileSize"/> values and returns an integer
        /// that indicates whether the first value is shorter than, equal to, or
        /// longer than the second value.
        /// </summary>
        /// <param name="f1">A <see cref="FileSize"/>.</param>
        /// <param name="f2">A FileSize.</param>
        /// <returns>Value Condition -1 <paramref name="f1"/> is shorter than
        /// <paramref name="f2"/> 0 <paramref name="f1"/> is equal to
        /// <paramref name="f2"/> 1 <paramref name="f1"/> is longer than
        /// <paramref name="f2"/>.</returns>
        public static int Compare(FileSize f1, FileSize f2)
        {
            if (f1.Bytes > f2.Bytes)
            {
                return 1;
            }

            if (f1.Bytes < f2.Bytes)
            {
                return -1;
            }

            return 0;
        }

        /// <summary>
        /// Returns a value indicating whether two specified instances of
        /// <see cref="FileSize"/> are equal.
        /// </summary>
        /// <param name="f1">A <see cref="FileSize"/>.</param>
        /// <param name="f2">A FileSize.</param>
        /// <returns>True if the values of <paramref name="f1"/> and
        /// <paramref name="f2"/> are equal; otherwise, false.</returns>
        public static bool Equals(FileSize f1, FileSize f2) => f1 == f2;

        /// <summary>
        /// Returns a <see cref="FileSize"/> that represents a specified size,
        /// where the specification is in units of bytes.
        /// </summary>
        /// <param name="value">A number of bytes that represent a file size.
        /// </param>
        /// <returns>A <see cref="FileSize"/> with a value of
        /// <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="value"/> is
        /// equal to <see cref="System.Double.NaN"/>.</exception>
        public static FileSize FromBytes(ulong value) => new FileSize(value);

        /// <summary>
        /// Returns a <see cref="FileSize"/> that represents a specified size of
        /// kilobytes, where the specification is accurate to the nearest byte.
        /// </summary>
        /// <param name="value">A number of kilobytes, accurate to the nearest
        /// byte.</param>
        /// <returns>A <see cref="FileSize"/> that represents
        /// <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="value"/> is
        /// equal to <see cref="System.Double.NaN"/>.</exception>
        public static FileSize FromKilobytes(double value) => ScaleDownToBytes(value, 1);

        /// <summary>
        /// Returns a <see cref="FileSize"/> that represents a specified size of
        /// megabytes, where the specification is accurate to the nearest byte.
        /// </summary>
        /// <param name="value">A number of megabytes, accurate to the nearest
        /// byte.</param>
        /// <returns>A <see cref="FileSize"/> that represents
        /// <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="value"/> is
        /// equal to <see cref="System.Double.NaN"/>.</exception>
        public static FileSize FromMegabytes(double value) => ScaleDownToBytes(value, 2);

        /// <summary>
        /// Returns a <see cref="FileSize"/> that represents a specified size of
        /// gigabytes, where the specification is accurate to the nearest byte.
        /// </summary>
        /// <param name="value">A number of gigabytes, accurate to the nearest
        /// byte.</param>
        /// <returns>A <see cref="FileSize"/> that represents
        /// <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="value"/> is
        /// equal to <see cref="System.Double.NaN"/>.</exception>
        public static FileSize FromGigabytes(double value) => ScaleDownToBytes(value, 3);

        /// <summary>
        /// Returns a <see cref="FileSize"/> that represents a specified size of
        /// terabytes, where the specification is accurate to the nearest byte.
        /// </summary>
        /// <param name="value">A number of terabytes, accurate to the nearest
        /// byte.</param>
        /// <returns>A <see cref="FileSize"/> that represents
        /// <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="value"/> is
        /// equal to <see cref="System.Double.NaN"/>.</exception>
        public static FileSize FromTerabytes(double value) => ScaleDownToBytes(value, 4);

        /// <summary>
        /// Returns a <see cref="FileSize"/> that represents a specified size of
        /// petabytes, where the specification is accurate to the nearest byte.
        /// </summary>
        /// <param name="value">A number of petabytes, accurate to the nearest
        /// byte.</param>
        /// <returns>A <see cref="FileSize"/> that represents
        /// <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="value"/> is
        /// equal to <see cref="System.Double.NaN"/>.</exception>
        public static FileSize FromPetabytes(double value) => ScaleDownToBytes(value, 5);

        /// <summary>
        /// Returns a <see cref="FileSize"/> that represents a specified size of
        /// exabytes, where the specification is accurate to the nearest byte.
        /// </summary>
        /// <param name="value">A number of exabytes, accurate to the nearest
        /// byte.</param>
        /// <returns>A <see cref="FileSize"/> that represents
        /// <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="value"/> is
        /// equal to <see cref="System.Double.NaN"/>.</exception>
        public static FileSize FromExabytes(double value) => ScaleDownToBytes(value, 6);

        #endregion Static Methods

        #region Operators

        /////// <summary>
        /////// Returns a <see cref="FileSize"/> whose value is the negated value of
        /////// the specified instance.
        /////// </summary>
        /////// <param name="f">A <see cref="FileSize"/>.</param>
        /////// <returns>Returns <see cref="FileSize"/> with the same numeric value
        /////// as this instance, but the opposite sign.</returns>
        /////// <exception cref="OverflowException">The negated value of this
        /////// instance cannot be represented by a <see cref="FileSize"/>; that is,
        /////// the value of this is <see cref="MinValue"/>.</exception>
        ////public static FileSize operator -(FileSize f)
        ////{
        ////    throw new OverflowException("Negating the value of an unsigned number is invalid.");
        ////}

        /// <summary>
        /// Subtracts the specified <see cref="FileSize"/> from another
        /// specified <see cref="FileSize"/>.
        /// </summary>
        /// <param name="f1">A <see cref="FileSize"/>.</param>
        /// <param name="f2">A FileSize.</param>
        /// <returns>A <see cref="FileSize"/> whose value is the result of the
        /// value of <paramref name="f1"/> minus the value of
        /// <paramref name="f2"/>.</returns>
        public static FileSize operator -(FileSize f1, FileSize f2) => f1.Subtract(f2);

        /// <summary>
        /// Returns the specified instance of <see cref="FileSize"/>.
        /// </summary>
        /// <param name="f">A <see cref="FileSize"/>.</param>
        /// <returns>Returns <paramref name="f"/>.</returns>
        public static FileSize operator +(FileSize f) => f;

        /// <summary>
        /// Adds two specified <see cref="FileSize"/> instances.
        /// </summary>
        /// <param name="f1">A <see cref="FileSize"/>.</param>
        /// <param name="f2">A FileSize.</param>
        /// <returns>A <see cref="FileSize"/> whose value is the sum of the
        /// values of <paramref name="f1"/> and <paramref name="f2"/>.</returns>
        public static FileSize operator +(FileSize f1, FileSize f2) => f1.Add(f2);

        /// <summary>
        /// Indicates whether two <see cref="FileSize"/> instance are equal.
        /// </summary>
        /// <param name="f1">A <see cref="FileSize"/>.</param>
        /// <param name="f2">A FileSize.</param>
        /// <returns>true if the values of <paramref name="f1"/> and
        /// <paramref name="f2"/> are equal; otherwise, false.</returns>
        public static bool operator ==(FileSize f1, FileSize f2) => f1.Bytes == f2.Bytes;

        /// <summary>
        /// Indicates whether two <see cref="FileSize"/> instance are not equal.
        /// </summary>
        /// <param name="f1">A <see cref="FileSize"/>.</param>
        /// <param name="f2">A FileSize.</param>
        /// <returns>True if the values of <paramref name="f1"/> and
        /// <paramref name="f2"/> are not equal; otherwise, false.</returns>
        public static bool operator !=(FileSize f1, FileSize f2) => f1.Bytes != f2.Bytes;

        /// <summary>
        /// Indicates whether a specified <see cref="FileSize"/> is less than
        /// another specified <see cref="FileSize"/>.
        /// </summary>
        /// <param name="f1">A <see cref="FileSize"/>.</param>
        /// <param name="f2">A FileSize.</param>
        /// <returns>True if the values of <paramref name="f1"/> is less than
        /// the value of <paramref name="f2"/>; otherwise, false.</returns>
        public static bool operator <(FileSize f1, FileSize f2) => f1.Bytes < f2.Bytes;

        /// <summary>
        /// Indicates whether a specified <see cref="FileSize"/> is less than or
        /// equal to another specified <see cref="FileSize"/>.
        /// </summary>
        /// <param name="f1">A <see cref="FileSize"/>.</param>
        /// <param name="f2">A FileSize.</param>
        /// <returns>True if the values of <paramref name="f1"/> is less than or
        /// equal to the value of <paramref name="f2"/>; otherwise, false.
        /// </returns>
        public static bool operator <=(FileSize f1, FileSize f2) => f1.Bytes <= f2.Bytes;

        /// <summary>
        /// Indicates whether a specified <see cref="FileSize"/> is greater than
        /// another specified <see cref="FileSize"/>.
        /// </summary>
        /// <param name="f1">A <see cref="FileSize"/>.</param>
        /// <param name="f2">A FileSize.</param>
        /// <returns>true if the values of <paramref name="f1"/> is greater than
        /// the value of <paramref name="f2"/>; otherwise, false.</returns>
        public static bool operator >(FileSize f1, FileSize f2) => f1.Bytes > f2.Bytes;

        /// <summary>
        /// Indicates whether a specified <see cref="FileSize"/> is greater than
        /// or equal to another specified <see cref="FileSize"/>.
        /// </summary>
        /// <param name="f1">A <see cref="FileSize"/>.</param>
        /// <param name="f2">A FileSize.</param>
        /// <returns>True if the values of <paramref name="f1"/> is greater than
        /// or equal to the value of <paramref name="f2"/>; otherwise, false.
        /// </returns>
        public static bool operator >=(FileSize f1, FileSize f2) => f1.Bytes >= f2.Bytes;

        #endregion Operators

        /// <summary>
        /// Adds the specified <see cref="FileSize"/> to this instance.
        /// </summary>
        /// <param name="fs">A <see cref="FileSize"/>.</param>
        /// <returns>A <see cref="FileSize"/> that represents the value of this
        /// instance plus the value of <paramref name="fs"/>.</returns>
        public FileSize Add(FileSize fs)
        {
            ulong bytes = this.Bytes + fs.Bytes;
            return new FileSize(bytes);
        }

        /// <summary>
        /// Compares the current instance with another object of the same type
        /// and returns an integer that indicates whether the current instance
        /// precedes, follows, or occurs in the same position in the sort order
        /// as the other object.
        /// </summary>
        /// <returns>
        /// A value that indicates the relative order of the objects being
        /// compared. The return value has these meanings: Value Meaning Less
        /// than zero This instance precedes <paramref name="obj"/> in the
        /// sort order. Zero This instance occurs in the same position in the
        /// sort order as <paramref name="obj"/>. Greater than zero This
        /// instance follows <paramref name="obj"/> in the sort order.
        /// </returns>
        /// <param name="obj">An object to compare with this instance.
        /// </param><exception cref="T:System.ArgumentException">
        /// <paramref name="obj"/> is not the same type as this instance.
        /// </exception>
        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }

            if (obj is FileSize fs)
            {
                return Compare(this, fs);
            }

            if (obj is ulong ul)
            {
                return Compare(this, new FileSize(ul));
            }

            throw ADP.ArgumentEx(nameof(obj), Resources.ERR_ObjectMustBeOfType.FormatWith("FileSize"));
        }

        /// <summary>
        /// Compares this instance to a specified <see cref="FileSize"/> object
        /// and returns an integer that indicates whether this instance is
        /// shorter than, equal to, or longer than the <see cref="FileSize"/>
        /// object.
        /// </summary>
        /// <param name="other">A <see cref="FileSize"/> object to compare to
        /// this instance.</param>
        /// <returns>A signed number indicating the relative values of this
        /// instance and <paramref name="other"/>. Value Description A negative
        /// integer This instance is shorter than <paramref name="other"/>. Zero
        /// This instance is equal to <paramref name="other"/>. A positive
        /// integer This instance is longer than <paramref name="other"/>.
        /// </returns>
        public int CompareTo(FileSize other) => Compare(this, other);

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a
        /// specified object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// True if <paramref name="obj"/> is a <see cref="FileSize"/>object
        /// that represents the same length as the current
        /// <see cref="FileSize"/> structure; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is FileSize size)
            {
                return this.Equals(size);
            }

            return obj is ulong bytes && this.Equals(bytes);
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a
        /// specified <see cref="FileSize"/> object.
        /// </summary>
        /// <param name="other">An <see cref="FileSize"/> object to compare with
        /// this instance.</param>
        /// <returns>True if <paramref name="other"/> represents the same file
        /// length as this instance; otherwise, false.</returns>
        public bool Equals(FileSize other) => this == other;

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a
        /// specified length.
        /// </summary>
        /// <param name="other">An length value to compare with this instance.
        /// </param>
        /// <returns>True if <paramref name="other"/> represents the same file
        /// length as this instance; otherwise, false.</returns>
        public bool Equals(ulong other) => this.Bytes == other;

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override int GetHashCode() => this.Bytes.GetHashCode();

        /////// <summary>
        /////// Returns a <see cref="FileSize"/> whose value is the negated value of
        /////// this instance.
        /////// </summary>
        /////// <returns>The same numeric value as this instance, but with the
        /////// opposite sign.</returns>
        /////// <exception cref="OverflowException">The negated value of this
        /////// instance cannot be represented by a <see cref="FileSize"/>; that is,
        /////// the value of this is <see cref="MinValue"/>.</exception>
        ////public FileSize Negate()
        ////{
        ////    return -this;
        ////}

        /// <summary>
        /// Subtracts the specified <see cref="FileSize"/> from this instance.
        /// </summary>
        /// <param name="fs">A <see cref="FileSize"/>.</param>
        /// <returns>A <see cref="FileSize"/> whose value is the result of the
        /// value of this instance minus the value of <paramref name="fs"/>.
        /// </returns>
        public FileSize Subtract(FileSize fs) => new FileSize(this.Bytes - fs.Bytes);

        /// <summary>
        /// Returns a string that represents this
        /// instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            double bytes = this.Bytes;
            int index = 0;

            while (bytes >= 1024)
            {
                bytes /= 1024;
                index += 1;
            }

            return $"{bytes:0.00} {FileSizeSuffix[index]}";
        }

        #endregion Methods

        #region Helpers

        /// <summary>
        /// Returns a <see cref="FileSize"/> that represents a specified size of
        /// value, where the specification is accurate to the nearest byte.
        /// </summary>
        /// <param name="value">The value to be scaled down.</param>
        /// <param name="scale">A value to which to scale the value.</param>
        /// <returns>A <see cref="FileSize"/> that represents
        /// <paramref name="value"/>.</returns>
        /// <exception cref="System.ArgumentException">FileSize does not accept
        /// floating point Not-a-Number values.</exception>
        private static FileSize ScaleDownToBytes(double value, int scale)
        {
            // Commented out due to testing not being able to make a condition
            // occur to warrant the need for the check.
            ////if (double.IsNaN(value))
            ////{
            ////    throw new ArgumentException("FileSize does not accept floating point Not-a-Number values.");
            ////}

            ////if (value < 1)
            ////{
            ////    return new FileSize();
            ////}

            double multiplier = Math.Pow(1024, scale);
            return new FileSize(Convert.ToUInt64(value * multiplier));
        }

        /// <summary>
        /// Scales bytes to the correct double value. Accurate to 10 decimal
        /// places.
        /// </summary>
        /// <param name="scale">The scaling value.</param>
        /// <returns>The scaled up value of bytes.</returns>
        private double ScaleUpFromBytes(int scale)
        {
            double value = this.Bytes / Math.Pow(1024, scale);
            return Math.Round(value, 10);
        }

        #endregion Helpers
    }
}