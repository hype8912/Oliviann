namespace Oliviann.Tests.TestObjects
{
    #region Usings

    using System;
    using System.Collections.Generic;

    #endregion Usings

    /// <summary>
    /// Represents a
    /// </summary>
    public static class TestValues
    {
        public const string NullString = null;

        public const object NullObject = null;

        public static IReadOnlyCollection<Tuple<object, bool, decimal, decimal?>> DecimalValues =>
        new List<Tuple<object, bool, decimal, decimal?>>
            {
                new Tuple<object, bool, decimal, decimal?>("21", true, (decimal)21.0, (decimal)21.0),
                new Tuple<object, bool, decimal, decimal?>("26.99", true, (decimal)26.99, (decimal)26.99),
                new Tuple<object, bool, decimal, decimal?>("A&*(", false, 0, null),
                new Tuple<object, bool, decimal, decimal?>("-21.42", true, (decimal)-21.42, (decimal)-21.42),
                new Tuple<object, bool, decimal, decimal?>(null, false, 0, null),
            };

        public static IReadOnlyCollection<Tuple<object, bool, short, short?>> Int16Values =>
            new List<Tuple<object, bool, short, short?>>
                {
                    new Tuple<object, bool, short, short?>("0", true, 0, 0),
                    new Tuple<object, bool, short, short?>("6", true, 6, 6),
                    new Tuple<object, bool, short, short?>("26.99", false, 0, null),
                    new Tuple<object, bool, short, short?>("A&*(", false, 0, null),
                    new Tuple<object, bool, short, short?>("-21.42", false, 0, null),
                    new Tuple<object, bool, short, short?>(null, false, 0, null),
                    new Tuple<object, bool, short, short?>("32767", true, 32767, 32767),
                    new Tuple<object, bool, short, short?>("-32768", true, -32768, -32768),
                };

        public static IReadOnlyCollection<Tuple<object, bool, int, int?>> Int32Values =>
            new List<Tuple<object, bool, int, int?>>
                {
                    new Tuple<object, bool, int, int?>("2147483647", true, 2147483647, 2147483647),
                    new Tuple<object, bool, int, int?>("-2147483648", true, -2147483648, -2147483648),
                };

        public static IReadOnlyCollection<Tuple<object, bool, long, long?>> Int64Values =>
            new List<Tuple<object, bool, long, long?>>
                {
                    new Tuple<object, bool, long, long?>("9223372036854775807", true, 9223372036854775807, 9223372036854775807),
                    new Tuple<object, bool, long, long?>("-9223372036854775808", true, -9223372036854775808, -9223372036854775808),
                };

        public static IReadOnlyCollection<Tuple<object, bool, ushort, ushort?>> UInt16Values =>
            new List<Tuple<object, bool, ushort, ushort?>>
                {
                    new Tuple<object, bool, ushort, ushort?>("0", true, 0, 0),
                    new Tuple<object, bool, ushort, ushort?>("6", true, 6, 6),
                    new Tuple<object, bool, ushort, ushort?>("26.99", false, 0, null),
                    new Tuple<object, bool, ushort, ushort?>("A&*(", false, 0, null),
                    new Tuple<object, bool, ushort, ushort?>("-21.42", false, 0, null),
                    new Tuple<object, bool, ushort, ushort?>(null, false, 0, null),
                    new Tuple<object, bool, ushort, ushort?>("65535", true, 65535, 65535),
                };

        public static IReadOnlyCollection<Tuple<object, bool, uint, uint?>> UInt32Values =>
            new List<Tuple<object, bool, uint, uint?>>
                {
                    new Tuple<object, bool, uint, uint?>("4294967295", true, 4294967295, 4294967295),
                };

        public static IReadOnlyCollection<Tuple<object, bool, ulong, ulong?>> UInt64Values =>
            new List<Tuple<object, bool, ulong, ulong?>>
                {
                    new Tuple<object, bool, ulong, ulong?>("18446744073709551615", true, 18446744073709551615, 18446744073709551615),
                };
    }
}