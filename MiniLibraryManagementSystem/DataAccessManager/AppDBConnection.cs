using DataBaseModels.DBModels;
using Microsoft.EntityFrameworkCore;

namespace DataAccessManager
{
    public class AppDBConnection : DbContext
    {
        public AppDBConnection(DbContextOptions<AppDBConnection> options) : base(options)
        {
        }

        public DbSet<BookManagementTable> bookManagementTables { get; set; }
        public DbSet<MemberManagementTable> memberManagementTables { get; set; }
        public DbSet<BorrowDetailsTable> borrowDetailsTables { get; set; }
        public DbSet<BorrowBookListTable> borrowBookListTables { get; set; }
        public DbSet<DueAttaterEmailLog> dueAttaterEmailLogs { get; set; }
    }
}
