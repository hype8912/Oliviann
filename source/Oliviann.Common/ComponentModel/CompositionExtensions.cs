#if NET35|| NET40 || NET45 || NET46 || NET47 || NET48

namespace Oliviann.ComponentModel
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.ComponentModel.Composition.Primitives;
    using System.Linq;
    using Oliviann.Collections.Generic;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// MEF.
    /// </summary>
    public static class CompositionExtensions
    {
        /// <summary>
        /// Determines whether the specified <paramref name="container" />
        /// contains a object for the specified type.
        /// </summary>
        /// <typeparam name="T">The type to look for in the exports container.
        /// </typeparam>
        /// <param name="container">The MEF composition container.</param>
        /// <param name="contractName">Name of the export contract. Optional.
        /// </param>
        /// <returns>
        /// True if the container contains an export for the specified type;
        /// otherwise, false.
        /// </returns>
        public static bool Contains<T>(this CompositionContainer container, string contractName = null)
        {
            ADP.CheckArgumentNull(container, nameof(container));
            if (contractName == null)
            {
                contractName = AttributedModelServices.GetContractName(typeof(T));
            }

            IEnumerable<Export> matches = container.GetExports(
                new ContractBasedImportDefinition(
                    contractName,
                    AttributedModelServices.GetTypeIdentity(typeof(T)),
                    Enumerable.Empty<string>() as IEnumerable<KeyValuePair<string, Type>>,
                    ImportCardinality.ZeroOrMore,
                    false,
                    false,
                    CreationPolicy.Any));

            return !matches.IsNullOrEmpty();
        }
    }
}

#endif