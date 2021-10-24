#if !NET35 && !NETSTANDARD1_3

namespace Oliviann.IPC
{
    #region Usings

    using System;
    using System.IO;
    using System.IO.MemoryMappedFiles;
    using Oliviann.Diagnostics;
    using Oliviann.IO;

    #endregion Usings

    /// <summary>
    /// Represents a class for launching a specified command by the inter
    /// process helper application. Used for running commands from a 64-bit
    /// process to a 32-bit process.
    /// </summary>
    public class InterProcessLauncher : IDisposable
    {
        #region Fields

        /// <summary>
        /// The current launcher process proxy.
        /// </summary>
        private readonly IProcessProxy process;

        /// <summary>
        /// Place holder for the memory mapped file instance.
        /// </summary>
        private MemoryMappedFile mappedFile;

        /// <summary>
        /// Place holder for the memory mapped file accessor instance.
        /// </summary>
        private MemoryMappedViewAccessor mappedAccessor;

        /// <summary>
        /// Place holder for the name of the memory mapped file.
        /// </summary>
        private string mappedFileName;

        #endregion Fields

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="InterProcessLauncher"/>
        /// class.
        /// </summary>
        public InterProcessLauncher() : this(new ProcessProxy())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InterProcessLauncher"/>
        /// class.
        /// </summary>
        /// <param name="processProxy">The process proxy.</param>
        internal InterProcessLauncher(IProcessProxy processProxy)
        {
            this.process = processProxy;
            this.Data = new InterProcessData();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="InterProcessLauncher"/>
        /// class.
        /// </summary>
        ~InterProcessLauncher() => this.Dispose(false);

        #endregion Constructor/Destructor

        #region Properties

        /// <summary>
        /// Gets or sets the command to be launched.
        /// </summary>
        /// <value>
        /// The command to be launched.
        /// </value>
        public string Command { get; set; }

        /// <summary>
        /// Gets the data to be passed in memory to the inter process helper.
        /// </summary>
        public InterProcessData Data { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing,
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Launches the inter process helper application for the specified
        /// command and then waits for the helper application to exit before
        /// returning.
        /// </summary>
        /// <exception cref="ArgumentNullException">The command argument is
        /// null.</exception>
        public void Launch()
        {
            ADP.CheckArgumentNullOrEmpty(this.Command, "Command");
            this.CreateNewMappedView();

            byte[] data = this.Data.ToStream().ToArray(true);
            this.mappedAccessor.WriteArray(0L, data, 0, data.Length);

            // Start the Process.
            // Wait for the Process to exit.
            this.LaunchHelper();

            // Destroy the accessor.
            // Destroy the mapped file.
            this.Dispose();
        }

        /// <summary>
        /// Launches the inter process helper application for the specified
        /// command and then returns without waiting on the helper application
        /// to exit. NOT IMPLEMENTED.
        /// </summary>
        public void LaunchAsync() => throw new NotImplementedException();

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged
        /// resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            this.process.DisposeSafe();
            this.mappedAccessor.DisposeSafe();
            this.mappedFile.DisposeSafe();
        }

        #endregion Methods

        #region Helpers

        /// <summary>
        /// Creates a new mapped view for communicating with the inter process
        /// application.
        /// </summary>
        private void CreateNewMappedView()
        {
            const int FileSize = 1 * 1024 * 1024;
            this.mappedFileName = StringHelpers.GenerateRandomString(12);

            this.mappedFile = MemoryMappedFile.CreateNew(this.mappedFileName, FileSize);
            this.mappedAccessor = this.mappedFile.CreateViewAccessor();
        }

        /// <summary>
        /// Launches the helper.
        /// </summary>
        private void LaunchHelper()
        {
            this.process.StartInfo.With(
                info =>
                    {
                        info.Arguments = $" {this.Command} {this.mappedFileName}";
                        info.UseShellExecute = true;
                        info.CreateNoWindow = true;
                        info.WorkingDirectory = Directory.GetCurrentDirectory();
                        info.FileName = @"Oliviann.Reuse.InterProcessHelper.exe";
                    });

            this.process.Start();
            this.process.WaitForExit();
        }

        #endregion Helpers
    }
}

#endif