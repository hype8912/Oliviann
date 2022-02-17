namespace Oliviann.Data
{
    #region Usings

    using System.Data.Odbc;
    using System.Data.OleDb;
#if NETFRAMEWORK
    using System.Data.OracleClient;
#endif
    using System.Data.SqlClient;
    using System.Xml.Serialization;

    #endregion Usings

    /// <summary>
    /// Database provider enumeration types.
    /// </summary>
    public enum DatabaseProvider
    {
        /// <summary>
        /// Signifies not database connection provider is available.
        /// </summary>
        [XmlEnum(Name = @"None")]
        None = 0,

        /////// <summary>
        /////// Database type for connecting to Windows Active Directory.
        /////// </summary>
        ////[XmlEnum(Name = @"ActiveDirectory")]
        ////[Provider(@"ADSDSOObject")]
        ////ActiveDirectory,

        /////// <summary>
        /////// Database type for connecting to AS/400 (IBM Series) databases.
        /////// </summary>
        ////[XmlEnum(Name = @"AS400IBM")]
        ////[Provider(@"IBMDA400")]
        ////AS400IBM,

        /////// <summary>
        /////// Database type for connecting to Cache database system.
        /////// </summary>
        ////[XmlEnum(Name = @"Cache")]
        ////[Provider(@"{InterSystems ODBC}")]
        ////Cache,

        /////// <summary>
        /////// Database type for connecting to a Composite Information database
        /////// server.
        /////// </summary>
        ////[XmlEnum(Name = @"CompInfoSrv")]
        ////[Provider(@"Composite 4.5")]
        ////CompositeInformationServer,

        /////// <summary>
        /////// Database type for connecting to a DBF/FoxPro server.
        /////// </summary>
        ////[XmlEnum(Name = @"DBFFoxPro")]
        ////[Provider(@"Microsoft.Jet.OLEDB.4.0")]
        ////DBFFoxPro,

        /////// <summary>
        /////// Database type for connecting to a DBMaker database.
        /////// </summary>
        ////[XmlEnum(Name = @"DBMaker")]
        ////[Provider(@"DMOLE43")]
        ////DBMaker,

        /////// <summary>
        /////// Database type for connecting to a DSN ODBC connection.
        /////// </summary>
        ////[XmlEnum(Name = @"DSN")]
        ////DSN,

        /////// <summary>
        /////// Database type for connecting to a Filemaker database.
        /////// </summary>
        ////[XmlEnum(Name = @"Filemaker")]
        ////[Provider(@"FileMaker Pro")]
        ////Filemaker,

        /////// <summary>
        /////// Database type for connecting to a Firebird database.
        /////// </summary>
        ////[XmlEnum(Name = @"Firebird")]
        ////Firebird,

        /// <summary>
        /// Generic odbc database provider.
        /// </summary>
        [XmlEnum(Name = "GenericOdbc")]
        [Provider("GenericOdbc", DbProviderFactoryType = typeof(OdbcFactory))]
        GenericOdbc,

        /// <summary>
        /// Generic oledb database provider.
        /// </summary>
        [XmlEnum(Name = "GenericOleDb")]
        [Provider("GenericOleDb", DbProviderFactoryType = typeof(OleDbFactory))]
        GenericOleDb,

        /////// <summary>
        /////// Database type for connecting to an HTML table.
        /////// </summary>
        ////[XmlEnum(Name = @"HtmlTable")]
        ////[Provider(@"Microsoft.Jet.OLEDB.4.0")]
        ////HtmlTable,

        /////// <summary>
        /////// Database type for connecting to an IBM DB2 database.
        /////// </summary>
        ////[XmlEnum(Name = @"IBMDB2")]
        ////IBMDB2,

        /////// <summary>
        /////// Database type for connecting to an IBM UniVerse or IBM UniData
        /////// database.
        /////// </summary>
        ////[XmlEnum(Name = @"IBMUni")]
        ////IBMUni,

        /////// <summary>
        /////// Database type for connecting to an Index Server database.
        /////// </summary>
        ////[XmlEnum(Name = @"IndexServer")]
        ////[Provider(@"MSIDXS")]
        ////IndexServer,

        /////// <summary>
        /////// Database type for connecting to an Informix database.
        /////// </summary>
        ////[XmlEnum(Name = @"Informix")]
        ////Informix,

        /////// <summary>
        /////// Database type for connecting to an Ingres database.
        /////// </summary>
        ////[XmlEnum(Name = @"Ingres")]
        ////[Provider(@"MSDASQL")]
        ////Ingres,

        /////// <summary>
        /////// Database type for connecting to an Interbase database.
        /////// </summary>
        ////[XmlEnum(Name = @"Interbase")]
        ////[Provider(@"sibprovider")]
        ////Interbase,

        /////// <summary>
        /////// Database type for connecting to an Intuit QuickBase database.
        /////// </summary>
        ////[XmlEnum(Name = @"QuickBase")]
        ////[Provider(@"{QuNect ODBC for QuickBase}")]
        ////IntuitQuickBase,

        /////// <summary>
        /////// Database type for connecting to Lotus Notes.
        /////// </summary>
        ////[XmlEnum(Name = @"LotusNotes")]
        ////[Provider(@"{Lotus NotesSQL 3.01 (32-bit) ODBC DRIVER (*.nsf)}")]
        ////LotusNotes,

        /////// <summary>
        /////// Database type for connecting to a Mimer SQL database.
        /////// </summary>
        ////[XmlEnum(Name = @"MimerSQL")]
        ////MimerSQL,

        /// <summary>
        /// Database type for connecting to Microsoft Access databases 2003 and
        /// lower.
        /// </summary>
        [XmlEnum(Name = "MSAccess")]
        [Provider("Microsoft.Jet.OLEDB.4.0", DbProviderFactoryType = typeof(OleDbFactory))]
        MicrosoftAccess,

        /// <summary>
        /// Database type for connecting to Microsoft Access databases 2007.
        /// </summary>
        [XmlEnum(Name = "MSAccess12")]
        [Provider("Microsoft.ACE.OLEDB.12.0", DbProviderFactoryType = typeof(OleDbFactory))]
        MicrosoftAccess12,

        /// <summary>
        /// Database type for connecting to Microsoft Access databases 2010.
        /// </summary>
        [XmlEnum(Name = "MSAccess14")]
        [Provider("Microsoft.ACE.OLEDB.14.0", DbProviderFactoryType = typeof(OleDbFactory))]
        MicrosoftAccess14,

        /// <summary>
        /// Database type for connecting to Microsoft Access databases 2013.
        /// </summary>
        [XmlEnum(Name = "MSAccess15")]
        [Provider("Microsoft.ACE.OLEDB.15.0", DbProviderFactoryType = typeof(OleDbFactory))]
        MicrosoftAccess15,

        /// <summary>
        /// Database type for connecting to Microsoft Access databases 2016.
        /// </summary>
        [XmlEnum(Name = "MSAccess16")]
        [Provider("Microsoft.ACE.OLEDB.16.0", DbProviderFactoryType = typeof(OleDbFactory))]
        MicrosoftAccess16,

        /// <summary>
        /// Database type for connecting to Microsoft Excel Workbook 2003 and
        /// lower.
        /// </summary>
        [XmlEnum(Name = "MSExcel")]
        [Provider("Microsoft.Jet.OLEDB.4.0", DbProviderFactoryType = typeof(OleDbFactory))]
        MicrosoftExcel,

        /// <summary>
        /// Database type for connecting to Microsoft Excel Workbook 2007.
        /// </summary>
        [XmlEnum(Name = "MSExcel12")]
        [Provider("Microsoft.ACE.OLEDB.12.0", DbProviderFactoryType = typeof(OleDbFactory))]
        MicrosoftExcel12,

        /// <summary>
        /// Database type for connecting to Microsoft Excel Workbook 2010.
        /// </summary>
        [XmlEnum(Name = "MSExcel14")]
        [Provider("Microsoft.ACE.OLEDB.14.0", DbProviderFactoryType = typeof(OleDbFactory))]
        MicrosoftExcel14,

        /// <summary>
        /// Database type for connecting to Microsoft Excel Workbook 2013.
        /// </summary>
        [XmlEnum(Name = "MSExcel15")]
        [Provider("Microsoft.ACE.OLEDB.15.0", DbProviderFactoryType = typeof(OleDbFactory))]
        MicrosoftExcel15,

        /////// <summary>
        /////// Database type for connecting to Microsoft Project 2000.
        /////// </summary>
        ////[XmlEnum(Name = @"MSProject")]
        ////[Provider(@"Microsoft.Project.OLEDB.9.0")]
        ////MicrosoftProject,

        /////// <summary>
        /////// Database type for connecting to Microsoft Project 2002.
        /////// </summary>
        ////[XmlEnum(Name = @"MSProject10")]
        ////[Provider(@"Microsoft.Project.OLEDB.10.0")]
        ////MicrosoftProject10,

        /////// <summary>
        /////// Database type for connecting to Microsoft Project 2003.
        /////// </summary>
        ////[XmlEnum(Name = @"MSProject11")]
        ////[Provider(@"Microsoft.Project.OLEDB.11.0")]
        ////MicrosoftProject11,

        /////// <summary>
        /////// Database type for connecting to Microsoft Sql Azure cloud database.
        /////// </summary>
        ////[XmlEnum(Name = @"MSSqlAzure")]
        ////MicrosoftSqlAzure,

        /// <summary>
        /// Database type for connecting to Microsoft SqlServer 7.0, 2000, 2005,
        /// 2008.
        /// </summary>
        [XmlEnum(Name = @"MSSqlServer")]
        [Provider("MSSqlServer", DbProviderFactoryType = typeof(SqlClientFactory))]
        MicrosoftSqlServer,

        /////// <summary>
        /////// Database type for connecting to a MySql database.
        /////// </summary>
        ////[XmlEnum(Name = @"MySql")]
        ////MySql,

        /////// <summary>
        /////// Database type for connecting to Microsoft Project 2002.
        /////// </summary>
        ////[XmlEnum(Name = @"NetezzaDBMS")]
        ////[Provider(@"NZOLEDB")]
        ////NetezzaDBMS,

        /////// <summary>
        /////// Database type for connecting to OLAP Analysis Services.
        /////// </summary>
        ////[XmlEnum(Name = "OLAPServices")]
        ////[Provider("MSOLAP")]
        ////OLAPAnalysisServices,

#if NETFRAMEWORK

        /// <summary>
        /// Database type for a Oracle connection.
        /// </summary>
        [XmlEnum(Name = @"Oracle")]
        [Provider("Oracle", DbProviderFactoryType = typeof(OracleClientFactory))]
        Oracle,

#endif

        /////// <summary>
        /////// Database type for connecting to a Paradox database.
        /////// </summary>
        ////[XmlEnum(Name = @"Paradox")]
        ////[Provider(@"Microsoft.Jet.OLEDB.4.0")]
        ////Paradox,

        /////// <summary>
        /////// Database type for connecting to a Pervasive database.
        /////// </summary>
        ////[XmlEnum(Name = @"Pervasive")]
        ////Pervasive,

        /////// <summary>
        /////// Database type for connecting to a Postgre SQL database.
        /////// </summary>
        ////[XmlEnum(Name = @"PostgreSQL")]
        ////PostgreSQL,

        /////// <summary>
        /////// Database type for connecting to a Progress database.
        /////// </summary>
        ////[XmlEnum(Name = @"Progress")]
        ////Progress,

        /////// <summary>
        /////// Database type for connecting to Microsoft SharePoint Services List.
        /////// </summary>
        ////[XmlEnum(Name = @"SharePoint12")]
        ////[Provider(@"Microsoft.ACE.OLEDB.12.0")]
        ////SharePoint12,

        /////// <summary>
        /////// Database type for connecting to a SQLBase database.
        /////// </summary>
        ////[XmlEnum(Name = @"SQLBase")]
        ////SQLBase,

        /// <summary>
        /// Database type for connecting to a SQLite database.
        /// </summary>
        [XmlEnum(Name = @"SQLite")]
        SQLite,

        /////// <summary>
        /////// Database type for connecting to a SyBase database.
        /////// </summary>
        ////[XmlEnum(Name = @"SyBase")]
        ////SyBase,

        /////// <summary>
        /////// Database type for connecting to a Teradata database.
        /////// </summary>
        ////[XmlEnum(Name = @"Teradata")]
        ////Teradata,

        /////// <summary>
        /////// Database type for connecting to a delimited text file.
        /////// </summary>
        ////[XmlEnum(Name = @"TextFile")]
        ////[Provider(@"Microsoft.Jet.OLEDB.4.0")]
        ////TextFile,

        /////// <summary>
        /////// Database type for connecting to a Valentina database.
        /////// </summary>
        ////[XmlEnum(Name = @"Valentina")]
        ////[Provider(@"{Valentina ODBC Driver}")]
        ////Valentina,

        /////// <summary>
        /////// Database type for connecting to a Vista DB database.
        /////// </summary>
        ////[XmlEnum(Name = @"VistaDB")]
        ////VistaDB,

        /////// <summary>
        /////// Database type for connecting to a Visual FoxPro/FoxPro 2.0 database.
        /////// </summary>
        ////[XmlEnum(Name = @"VisualFoxPro")]
        ////[Provider(@"vfpoledb")]
        ////VisualFoxPro,
    }
}