using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataBaseModels.DBModels
{
    [Table("DueAttaterEmailLog")]
    public class DueAttaterEmailLog
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? Message { get; set; }
        public DateTime SendDate { get; set; }
    }

}
