namespace Oliviann.IO.Compression.Tests
{
    #region Usings

    using System;
    using System.IO;
    using System.Reflection;
    using Oliviann.Reflection;

    #endregion Usings

    /// <summary>
    /// Represents a base XUnit class fixture for running Seven Zip tests.
    /// </summary>
    public class SevenZipCompressionTestsFixture : IDisposable
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="SevenZipCompressionTestsFixture"/> class.
        /// </summary>
        public SevenZipCompressionTestsFixture()
        {
            string randDirName = Path.GetFileNameWithoutExtension(Path.GetTempFileName());
            string tempFolderPath = Path.Combine(Path.GetTempPath(), randDirName);
            Directory.CreateDirectory(tempFolderPath);
            string currentDir = Path.Combine(Assembly.GetExecutingAssembly().GetCurrentExecutingDirectory(), "TestObjects");

            this.TemporaryDirPath = tempFolderPath;

            this.TemporaryArchivePath_Small = Path.Combine(this.TemporaryDirPath, "TestArchive.7z");
            File.Copy(Path.Combine(currentDir, @"TestArchive.7z"), this.TemporaryArchivePath_Small);

            this.TemporaryArchivePath_Small_NoPassword = Path.Combine(this.TemporaryDirPath, "TestArchive_NoPass.7z");
            File.Copy(Path.Combine(currentDir, @"TestArchive_NoPass.7z"), this.TemporaryArchivePath_Small_NoPassword);

            this.TemporaryArchivePath_Medium = Path.Combine(this.TemporaryDirPath, @"IMIS_5_1.TEST.ipdu");
            File.Copy(Path.Combine(currentDir, @"IMIS_5_1.TEST.ipdu"), this.TemporaryArchivePath_Medium);
        }

        #endregion Constructor/Destructor

        #region Properties

        public string TemporaryDirPath { get; set; }

        public string TemporaryArchivePath_Small { get; set; }

        public string TemporaryArchivePath_Small_NoPassword { get; set; }

        public string TemporaryArchivePath_Medium { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing,
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (!this.TemporaryDirPath.IsNullOrEmpty() && Directory.Exists(this.TemporaryDirPath))
            {
                Directory.Delete(this.TemporaryDirPath, true);
            }

            GC.SuppressFinalize(this);
        }

        #endregion Methods
    }
}