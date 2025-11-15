using ClassRecord;
using ClassRecord.ReportManagement;
using DataAccessManager;
using EnumClasses;

namespace ServiceManager.ReportManagement
{
    public interface IReportManagementServices
    {
        public Task<ReturnRecord> GenerateReport(DateTime fromDate, DateTime toDate);
    }
    public class ReportManagementServices : IReportManagementServices
    {
        public async Task<ReturnRecord> GenerateReport(DateTime fromDate, DateTime toDate)
        {
            string query = "SELECT T.BORROWDATE, " +
                                  "T.DUEDATE, " +
                                  "T.RETURNDATE, " +
                                  "MT.FULLNAME, " +
                                  "BBLT.BOOKID, " +
                                  "BMT.TITLE, " +
                                  "BMT.AUTHOR, " +
                                  "BMT.CATEGORY " +
                           "FROM BorrowDetailsTable T " +
                           "LEFT JOIN MemberManagementTable MT " +
                           "ON MT.MEMBERID = T.MEMBERID " +
                           "LEFT JOIN BorrowBookListTable BBLT " +
                           "ON BBLT.BORROWID = T.BORROWID " +
                           "LEFT JOIN BookManagementTable BMT " +
                           "ON BMT.BOOKID = BBLT.BOOKID " +
                           "WHERE T.ISDELETE != 1;";

            var reportData = await DapperDataAccessManager.QueryList<AllBorrowBookListModel>(query);

            var selectedData = reportData.Where(x=>x.BorrowDate >= fromDate && x.BorrowDate <= toDate);

            var totalBooksBorrowed = selectedData.Count();
            var totalBooksReturned = selectedData.Count(x => x.ReturnDate != null);
            var totalActiveBorrowings = totalBooksBorrowed - totalBooksReturned;

            var bestBook = selectedData
                .GroupBy(x => x.BookId)
                .Select(group => new
                {
                    Title = group.First().Title,
                    Category = group.First().Category,
                    Author = group.First().Author,
                    BorrowCount = group.Count()
                })
                .OrderByDescending(x => x.BorrowCount)
                .FirstOrDefault();

            return new ReturnRecord
            (
                new
                {
                    Total_Books_Borrowed = totalBooksBorrowed,
                    Total_Books_Returned = totalBooksReturned,
                    Active_Borrow_Records = totalActiveBorrowings,
                    Most_Borrowed_Book = bestBook
                },
                "Report generated successfully.",
                
                ResultStatus.Success
            );

        }
    }
}
