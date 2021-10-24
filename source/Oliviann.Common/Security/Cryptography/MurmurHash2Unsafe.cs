//-----------------------------------------------------------------------
// <copyright file="MurmurHash2Unsafe.cs">
//     Version: MPL 1.1/GPL 2.0/LGPL 2.1
//
//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis,WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations under
//     the License.
//
//     The Original Code is HashTableHashing.MurmurHash2.
//
//     The Initial Developer of the Original Code is Davy Landman.
//     Portions created by the Initial Developer are Copyright (C) 2009
//     the Initial Developer. All Rights Reserved.
//     http://landman-code.blogspot.com/2009/02/c-superfasthash-and-murmurhash2.html
//
//     Contributor(s):
//
//     Alternatively, the contents of this file may be used under the terms of
//     either the GNU General Public License Version 2 or later (the "GPL"), or
//     the GNU Lesser General Public License Version 2.1 or later (the "LGPL"),
//     in which case the provisions of the GPL or the LGPL are applicable
//     instead of those above. If you wish to allow use of your version of this
//     file only under the terms of either the GPL or the LGPL, and not to allow
//     others to use your version of this file under the terms of the MPL,
//     indicate your decision by deleting the provisions above and replace them
//     with the notice and other provisions required by the GPL or the LGPL. If
//     you do not delete the provisions above, a recipient may use your version
//     of this file under the terms of any one of the MPL, the GPL or the LGPL.
// </copyright>
//-----------------------------------------------------------------------

namespace Oliviann.Security.Cryptography
{
    /// <summary>
    /// Represents a C# implementation of the MurmurHash2 using pointers
    /// originally written in C++. This version is about 33% faster than the
    /// version that doesn't use pointers.
    /// </summary>
    public class MurmurHash2Unsafe : ISeededHashAlgorithm
    {
        #region Fields

        /// <summary>
        /// Mixing constant m.
        /// </summary>
        private const uint M = 0x5bd1e995;

        /// <summary>
        /// Mixing constant r.
        /// </summary>
        private const int R = 24;

        #endregion Fields

        /// <summary>
        /// Hashes the specified data.
        /// </summary>
        /// <param name="data">The byte array to be hashed.</param>
        /// <returns>
        /// The hash code value for the specified data.
        /// </returns>
        public uint Hash(byte[] data)
        {
            return this.Hash(data, 0xc58f1a7b);
        }

        /// <summary>
        /// Hashes the specified data using the specified hash
        /// <paramref name="seed"/> value.
        /// </summary>
        /// <param name="data">The byte array to be hashed.</param>
        /// <param name="seed">The value to seed the hashing algorithm.</param>
        /// <returns>
        /// The hash code value for the specified data.
        /// </returns>
        public unsafe uint Hash(byte[] data, uint seed)
        {
            int length = data.Length;
            if (length < 1)
            {
                return 0;
            }

            // Initialize the hash to a random value.
            uint h = seed ^ (uint)length;

            int remainingBytes = length & 3; // mod 4
            int numberOfLoops = length >> 2; // div 4
            fixed (byte* firstByte = &data[0])
            {
                var realData = (uint*)firstByte;
                while (numberOfLoops != 0)
                {
                    uint k = *realData * M;
                    k ^= k >> R;
                    k *= M;

                    h *= M;
                    h ^= k;
                    numberOfLoops--;
                    realData += 1;
                }

                // Handle the last few bytes of the input array.
                switch (remainingBytes)
                {
                    case 3:
                        h ^= (ushort)(*realData);
                        h ^= ((uint) * (((byte*)realData) + 2)) << 16;
                        h *= M;
                        break;

                    case 2:
                        h ^= (ushort)(*realData);
                        h *= M;
                        break;

                    case 1:
                        h ^= *((byte*)realData);
                        h *= M;
                        break;
                }
            }

            // Do a few final mixes of the hash to ensure the last few bytes are
            // well-incorporated.
            h = h.MixFinalMurmur2();
            return h;
        }
    }
}