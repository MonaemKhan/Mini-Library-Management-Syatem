using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataBaseModels.DBModels
{
    [Table("MemberManagementTable")]
    public class MemberManagementTable : ICommonProperty
    {
        [Key]
        public int MemberId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public DateTime JoinDate { get; set; }
        public int IsActive { get; set; }
        public int IsDelete { get; set; } = 0;
    }

}
