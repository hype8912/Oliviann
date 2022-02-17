namespace Oliviann.InterProcessHelper
{
    #region Usings

    using System;
    using System.Diagnostics;
    using System.IO.MemoryMappedFiles;
    using System.Runtime.Serialization.Formatters.Binary;
    using Oliviann.IPC;
    using Oliviann.Runtime.Serialization.Formatters.Binary;

    #endregion Usings

    /// <summary>
    /// Represents a class for handling validation and storage of arguments.
    /// </summary>
    internal static class Arguments
    {
        #region Fields

        /// <summary>
        /// Trace source for this assembly.
        /// </summary>
        internal static readonly TraceSource Source = new("Oliviann");

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets the command to be executed.
        /// </summary>
        internal static string Command { get; private set; }

        /// <summary>
        /// Gets the name of the memory mapped file.
        /// </summary>
        /// <value>
        /// The name of the memory mapped file.
        /// </value>
        internal static string MappedFileName { get; private set; }

        /// <summary>
        /// Gets the deserialized data object from the memory mapped file.
        /// </summary>
        internal static InterProcessData Data { get; private set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Sets the arguments to properties for shared retrieval.
        /// </summary>
        /// <param name="args">The args.</param>
        internal static void SetArguments(string[] args)
        {
            Command = args[0];
            MappedFileName = args[1];

            ReadMemoryMappedFile();
        }

        /// <summary>
        /// Reads the memory mapped file and sets the deserialized data to the
        /// Data argument.
        /// </summary>
        private static void ReadMemoryMappedFile()
        {
            if (MappedFileName.IsNullOrEmpty())
            {
                return;
            }

            try
            {
                using (MemoryMappedFile mappedFile = MemoryMappedFile.OpenExisting(MappedFileName))
                using (MemoryMappedViewStream stream = mappedFile.CreateViewStream())
                {
                    Data = new BinaryFormatter().Deserialize<InterProcessData>(stream);
                }
            }
            catch (Exception ex)
            {
                Source.TraceEvent(
                    TraceEventType.Error,
                    6001,
                    "An error occurred trying to read MemoryMappedFile: {0}{1}{2}",
                    MappedFileName,
                    Environment.NewLine,
                    ex);
            }
        }

        #endregion Methods
    }
}