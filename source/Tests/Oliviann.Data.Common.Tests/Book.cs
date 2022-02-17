namespace Oliviann.Data.Common.Tests
{
    #region Usings

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Diagnostics.CodeAnalysis;

    #endregion Usings

    /// <summary>
    /// Represents a
    /// </summary>
    [Table("Books")]
    [ExcludeFromCodeCoverage]
    public class Book
    {
        #region Properties

        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("author", Order = 1)]
        public string Author { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("year")]
        public uint Year { get; set; }

        [NotMapped]
        public string Notes { get; set; }

        #endregion Properties
    }
}