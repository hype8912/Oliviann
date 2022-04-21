namespace Oliviann.Numerics
{
    #region Usings

    using System.Runtime.CompilerServices;

    #endregion

    /// <summary>
    /// Represents a class of methods for intrinsic bit-twiddling operations.
    /// </summary>
    /// <remarks>https://github.com/dotnet/corert/blob/master/src/System.Private.CoreLib/shared/System/Numerics/BitOperations.cs</remarks>
    internal static class BitOperations
    {
        #region Methods

        /// <summary>
        /// Rotates the specified value left by the specified number of bits.
        /// Similar in behavior to the x86 instruction ROL.
        /// </summary>
        /// <param name="value">The value to rotate.</param>
        /// <param name="offset">The number of bits to rotate by.
        /// Any value outside the range [0..31] is treated as congruent mod 32.
        /// </param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(256)]
        public static uint RotateLeft(uint value, int offset)
            => (value << offset) | (value >> (32 - offset));

        #endregion
    }
}