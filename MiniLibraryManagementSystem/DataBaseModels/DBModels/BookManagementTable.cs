using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataBaseModels.DBModels
{
    [Table("BookManagementTable")]
    public class BookManagementTable : ICommonProperty
    {
        [Key]
        public int BookId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Isbn { get; set; } = string.Empty;
        public string? Category { get; set; }
        public int CopiesAvailable { get; set; }
        public int PublishedYear { get; set; }
        public int Status { get; set; }
        public int IsDelete { get; set; } = 0;
    }

}
