namespace Oliviann.IO.Compression
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using ComponentModel;
    using Properties;

    #endregion Usings

    /// <summary>
    /// Represents a class for compressing and decompressing archive using a
    /// 7zip wrapper.
    /// </summary>
    public class SevenZipCompression2 : IDisposable
    {
        #region Fields

        /// <summary>
        /// Class name for reporting purposes.
        /// </summary>
        private const string ClassName = "SevenZipCompression2";

        /// <summary>
        /// Place holder for the current running process.
        /// </summary>
        private Process currentProcess;

        #endregion Fields

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SevenZipCompression2"/>
        /// class.
        /// </summary>
        /// <param name="archiveFilePath">The file path to the archive file or
        /// where the archive file will be created.</param>
        /// <param name="command">The compression command.</param>
        public SevenZipCompression2(string archiveFilePath, CompressionCommand command)
        {
            ADP.CheckArgumentNullOrEmpty(archiveFilePath, nameof(archiveFilePath));
            this.Arguments = new CompressionArguments { ArchiveFilePath = archiveFilePath, Command = command };
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="SevenZipCompression2"/>
        /// class.
        /// </summary>
        ~SevenZipCompression2()
        {
            this.Dispose(false);
        }

        #endregion Constructor/Destructor

        #region Events

        /// <summary>
        /// Occurs when file extraction has started.
        /// </summary>
        public event EventHandler<EventArgs<string>> FileStarted;

        #endregion Events

        #region Properties

        /// <summary>
        /// Gets the arguments for launching the 7-zip application.
        /// </summary>
        public CompressionArguments Arguments { get; private set; }

        #endregion Properties

        /// <summary>
        /// Compresses an archive using the specified arguments.
        /// </summary>
        /// <returns>
        /// True if the archive compression completed successfully; otherwise,
        /// false.
        /// </returns>
        public bool CompressArchive()
        {
            this.CheckCurrentProcess();

            try
            {
                this.currentProcess = new Process();

                // Execute compression.
                this.ExecuteArchiveAction();

                // Wait for application to exit.
                this.WaitTillProcessCloses();
            }
            finally
            {
                this.currentProcess.DisposeSafe();
            }

            this.currentProcess = null;
            Trace.WriteLine(@"Detailed: ");

            return !this.Arguments.ArchiveFilePath.IsNullOrEmpty() &&
                    File.Exists(this.Arguments.ArchiveFilePath);
        }

        /// <summary>
        /// Decompresses the complete archive's contents to the specified output
        /// path.
        /// </summary>
        /// <returns>An exit code returned by the compression application.
        /// </returns>
        public int DecompressArchive()
        {
            this.CheckCurrentProcess();

            try
            {
                this.currentProcess = new Process();

                // Execute decompression.
                this.ExecuteArchiveAction();

                // Wait for application to exit.
                this.WaitTillProcessCloses();
            }
            finally
            {
                this.currentProcess.DisposeSafe();
            }

            this.currentProcess = null;
            Trace.WriteLine(@"Detailed: ");
            return 0;
        }

        /// <summary>
        /// Decompresses a single file from the archive to the specified output
        /// path.
        /// </summary>
        /// <param name="filePath">The file path to be extracted.</param>
        /// <returns>
        /// An exit code returned by the compression application.
        /// </returns>
        /// <exception cref="CompressionException">An error occurred while
        /// decompressing file. See inner exception for more information.
        /// </exception>
        /// <exception cref="FileNotFoundException">The specified file path does
        /// not exist in the current archive.</exception>
        public int DecompressFile(string filePath)
        {
            this.CheckCurrentProcess();

            try
            {
                ADP.CheckArgumentNullOrEmpty(filePath, nameof(filePath));
                Debug.WriteLine("Request extraction of file: {0}".FormatWith(filePath), ClassName);

                IList<string> fileNames = this.ReadArchiveFileNames();
                if (!fileNames.Contains(filePath))
                {
                    throw new FileNotFoundException(Resources.FileNotFoundInArchiveError, filePath);
                }

                this.Arguments.IncludeFileNames = new List<string> { filePath };
                this.currentProcess = new Process();

                // Execute decompression.
                this.ExecuteArchiveAction();

                // Wait for application to exit.
                this.WaitTillProcessCloses();
            }
            catch (Exception ex)
            {
                throw new CompressionException(Resources.DecompressionError, ex);
            }
            finally
            {
                this.currentProcess.DisposeSafe();
            }

            this.currentProcess = null;
            return 0;
        }

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
        /// Gets the file path location to the 7zip library.
        /// </summary>
        /// <returns>
        /// The file path location to the 7zip library if found; otherwise,
        /// null.
        /// </returns>
        /// <exception cref="CompressionException">Argument is null or empty.
        /// See inner exception for more information.</exception>
        public string GetLibraryPath()
        {
            // First tries to get a custom application path.
            string libPath = this.Get7zipApplicationCustomPath();
            if (!libPath.IsNullOrEmpty())
            {
                Debug.WriteLine("7zip Library Path = {0}".FormatWith(libPath), ClassName);
                return libPath;
            }

            // No custom path was found so now we try to get the application
            // path from the assembly path location.
            libPath = this.Get7zipApplicationAssemblyPath();
            if (!libPath.IsNullOrEmpty())
            {
                Debug.WriteLine("7zip Library Path = {0}".FormatWith(libPath), ClassName);
                return libPath;
            }

#if !NET35
            // No assembly path library was found so we try to get the library
            // path from the installed location path.
            libPath = this.Get7zipApplicationInstallPath();
            if (!libPath.IsNullOrEmpty())
            {
                Debug.WriteLine("7zip Library Path = {0}".FormatWith(libPath), ClassName);
                return libPath;
            }
#endif

            throw new CompressionException(Resources.CompressionError);
        }

        /// <summary>
        /// Determines whether the archive file is password protected.
        /// </summary>
        /// <returns>
        /// True if the archive file is password protected; otherwise, false.
        /// </returns>
        public bool IsArchivePasswordProtected()
        {
            bool passwordProtected = false;
            this.CheckCurrentProcess();

            try
            {
                this.currentProcess = new Process();
                ICompressionParser parser = CompressionFactory.GetCompressionParser(CompressionCommand.Test, this.Arguments);
                if (parser != null)
                {
                    this.currentProcess.StartInfo.RedirectStandardOutput = true;
                    this.currentProcess.StartInfo.RedirectStandardError = true;
                    this.currentProcess.OutputDataReceived += parser.OutDataReceivedParser;
                    this.currentProcess.ErrorDataReceived += parser.OutDataReceivedParser;
                    parser.FileStarted += (s, e) => this.currentProcess.DisposeSafe();
                    parser.ExceptionRelay = ex1 =>
                                                {
                                                    this.currentProcess.DisposeSafe();
                                                    passwordProtected = true;
                                                };
                }

                this.currentProcess.StartInfo.FileName = this.GetLibraryPath();
                this.currentProcess.StartInfo.UseShellExecute = false;

                ICompressionCLIBuilder builder = CompressionFactory.GetCommandLineBuilder(CompressionCommand.Test);
                if (builder != null)
                {
                    CompressionArguments tempArguments = this.Arguments.Copy();
                    tempArguments.Password = Settings.Default.TestArchivePassword;
                    this.currentProcess.StartInfo.Arguments = builder.BuildCommandArguments(tempArguments);
                }

                this.currentProcess.Start();
                if (parser != null)
                {
                    this.currentProcess.BeginOutputReadLine();
                    this.currentProcess.BeginErrorReadLine();
                }

                this.currentProcess.WaitForExit();
                this.WaitTillProcessCloses();
            }
            catch (CompressionException ex)
            {
                if (ex.GetBaseException().GetType() == typeof(UnauthorizedAccessException))
                {
                    passwordProtected = true;
                }
            }
            finally
            {
                this.currentProcess.DisposeSafe();
            }

            this.currentProcess = null;
            return passwordProtected;
        }

        /// <summary>
        /// Reads the archive and returns a collection of files contained inside
        /// the archive.
        /// </summary>
        /// <returns>
        /// A collection of files contained inside the archive.
        /// </returns>
        public IList<string> ReadArchiveFileNames()
        {
            this.CheckCurrentProcess();
            IList<string> files = new List<string>();

            try
            {
                this.currentProcess = new Process();
                ICompressionParser parser = this.ExecuteArchiveAction(CompressionCommand.List);

                this.WaitTillProcessCloses(5);
                if (parser != null && parser.ArchiveFiles != null)
                {
                    files = parser.ArchiveFiles.Select(f => f.FileName).ToList();
                }
            }
            finally
            {
                this.currentProcess.DisposeSafe();
            }

            return files;
        }

        #region Internal Workers

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged
        /// resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            this.currentProcess.DisposeSafe();
        }

        /// <summary>
        /// Executes the archive command action for the current arguments.
        /// </summary>
        /// <returns>A new <see cref="ICompressionParser"/> used for parsing the
        /// output data.</returns>
        private ICompressionParser ExecuteArchiveAction()
        {
            return this.ExecuteArchiveAction(this.Arguments.Command);
        }

        /// <summary>
        /// Executes the archive command action for the current arguments.
        /// </summary>
        /// <param name="command">The compression command to execute.</param>
        /// <returns>
        /// A new <see cref="ICompressionParser" /> used for parsing the
        /// output data.
        /// </returns>
        private ICompressionParser ExecuteArchiveAction(CompressionCommand command)
        {
            ICompressionParser parser = CompressionFactory.GetCompressionParser(command, this.Arguments);
            if (parser != null)
            {
                this.currentProcess.StartInfo.RedirectStandardOutput = true;
                this.currentProcess.StartInfo.RedirectStandardError = true;
                this.currentProcess.OutputDataReceived += parser.OutDataReceivedParser;
                this.currentProcess.ErrorDataReceived += parser.OutDataReceivedParser;
                parser.FileStarted += this.FileStarted;
            }

            this.currentProcess.StartInfo.FileName = this.GetLibraryPath();
            this.currentProcess.StartInfo.UseShellExecute = false;

            ICompressionCLIBuilder builder = CompressionFactory.GetCommandLineBuilder(command);
            if (builder != null)
            {
                this.currentProcess.StartInfo.Arguments = builder.BuildCommandArguments(this.Arguments);
                Debug.WriteLine("Process arguments = '{0}'".FormatWith(this.currentProcess.StartInfo.Arguments), ClassName);
            }

#if !DEBUG
            this.currentProcess.StartInfo.CreateNoWindow = true;
            this.currentProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
#endif

            this.currentProcess.Start();
            if (parser != null)
            {
                this.currentProcess.BeginOutputReadLine();
                this.currentProcess.BeginErrorReadLine();
            }

            this.currentProcess.WaitForExit();
            return parser;
        }

        #endregion Internal Workers

        #region Helpers

        /// <summary>
        /// Checks the current process to ensure multiple processes aren't
        /// running.
        /// </summary>
        /// <exception cref="CompressionException">Access violation error.
        /// </exception>
        private void CheckCurrentProcess()
        {
            if (this.currentProcess != null)
            {
                var ex = new UnauthorizedAccessException(Resources.MultipleProcessesError);
                throw new CompressionException(Resources.AccessViolationError, ex);
            }
        }

        /// <summary>
        /// Gets the custom specified library path if one is specified by the
        /// developer.
        /// </summary>
        /// <returns>A path to the 7zip application file if found; otherwise,
        /// null.</returns>
        private string Get7zipApplicationCustomPath()
        {
            if (this.Arguments.ApplicationFilePath.IsNullOrEmpty() || !File.Exists(this.Arguments.ApplicationFilePath))
            {
                return null;
            }

            return this.Arguments.ApplicationFilePath;
        }

#if !NET35

        /// <summary>
        /// Tries to find the 7zip library path based on the user having 7zip
        /// installed on the current machine.
        /// </summary>
        /// <returns>A path to the 7zip library file if found; otherwise, null.
        /// </returns>
        private string Get7zipApplicationInstallPath()
        {
            string libPath = Settings.Default.LibraryPath;

            if (!File.Exists(libPath))
            {
                bool is64BitOS = Environment.Is64BitOperatingSystem;
                if (is64BitOS)
                {
                    // Give the 64 bit operating system a second chance and look
                    // in the 32 bit Program Files location for the library.
                    libPath = Settings.Default.Library32bitPath;
                    if (!File.Exists(libPath))
                    {
                        // Couldn't find a 32 bit 7z.exe file in the 32 bit
                        // Program Files directory.
                        libPath = null;
                    }
                }
                else
                {
                    libPath = null;
                }
            }

            return libPath;
        }

#endif

        /// <summary>
        /// Tries to find the 7zip command line application path based on the
        /// path of the executing assembly.
        /// </summary>
        /// <returns>A path to the 7zip command line application file if found;
        /// otherwise, null.</returns>
        private string Get7zipApplicationAssemblyPath()
        {
            string assemblyBase = Assembly.GetExecutingAssembly().CodeBase;
            string assemblyLocation = assemblyBase.Replace("file:///", string.Empty);
            string libDirectory = Path.GetDirectoryName(assemblyLocation).AddPathSeparator();
            string libPath = libDirectory + Settings.Default.ApplicationFileName;

            if (!File.Exists(libPath))
            {
                libPath = null;
            }

            return libPath;
        }

        /// <summary>
        /// Waits till the process closes and then returns.
        /// </summary>
        /// <param name="secondsToWait">The number of seconds to wait before
        /// checking the application has exited. Default value is 8 seconds.
        /// </param>
        private void WaitTillProcessCloses(int secondsToWait = 8)
        {
            int processId;
            try
            {
                processId = this.currentProcess.Id;
            }
            catch (InvalidOperationException ex)
            {
                Debug.WriteLine("Exception thrown retrieving the current process Id. " + ex, ClassName);
                return;
            }

            while (Process.GetProcesses().Any(p => p.Id == processId))
            {
                Thread.Sleep(TimeSpan.FromSeconds(secondsToWait));
            }
        }

        #endregion Helpers
    }
}