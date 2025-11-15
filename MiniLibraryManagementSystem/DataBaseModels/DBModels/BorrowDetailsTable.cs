using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataBaseModels.DBModels
{
    [Table("BorrowDetailsTable")]
    public class BorrowDetailsTable : ICommonProperty
    {
        [Key]
        public int BorrowId { get; set; }
        public int MemberId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public int IsDelete { get; set; } = 0;
    }

}
