using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataBaseModels.DBModels
{
    [Table("BorrowBookListTable")]
    public class BorrowBookListTable : ICommonProperty
    {
        [Key]
        public int Id { get; set; }
        public int BorrowId { get; set; }
        public int BookId { get; set; }
        public int IsDelete { get; set; } = 0;
    }
}
