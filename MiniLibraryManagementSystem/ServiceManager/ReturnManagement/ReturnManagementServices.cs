using ClassRecord;
using ClassRecord.ReturnManagement;
using DataAccessManager;
using DataBaseModels.DBModels;
using EnumClasses;
using Microsoft.Extensions.Configuration;
using ModelValidateAndConvert.ReturnManagement;
using ServiceManager.BookManagement;

namespace ServiceManager.ReturnManagement
{
    public interface IReturnManagementServices
    {
        public Task<ReturnRecord> ReturnBorrowBook(ReturnBorrowedBookRecord returnBorrowRecord);
    }
    public class ReturnManagementServices : IReturnManagementServices
    {
        private readonly IEFCoreDataAccessManager<BorrowDetailsTable> _dataAccess;
        private readonly IBookManagementServices _bookManagementServices;
        private readonly IConfiguration _configuration;
        public ReturnManagementServices(IEFCoreDataAccessManager<BorrowDetailsTable> dataAccess,
            IBookManagementServices bookManagementServices,
            IConfiguration configuration)
        {
            _dataAccess = dataAccess;
            _bookManagementServices = bookManagementServices;
            _configuration = configuration;
        }

        public async Task<ReturnRecord> ReturnBorrowBook(ReturnBorrowedBookRecord returnBorrowRecord)
        {
            try
            {
                ReturnManagementBorrowDetailsUpdate returnObj = new ReturnManagementBorrowDetailsUpdate(returnBorrowRecord);
                if (!returnObj.IsValid())
                {
                    return new ReturnRecord(string.Empty, returnObj.GetErrorMessage(), ResultStatus.Failure);
                }
                var borrowDetails = await _dataAccess.GetByIdAsync(returnBorrowRecord.BORROWID);
                if (borrowDetails == null
                    || borrowDetails.IsDelete == (int)DeleteStatus.Delete)
                {
                    return new ReturnRecord(string.Empty, "Borrow Details Not Found", ResultStatus.Failure);
                }
                else if (borrowDetails.ReturnDate != null)
                {
                    return new ReturnRecord(string.Empty, "Book Already Returned", ResultStatus.Failure);
                }

                var BookList = await DapperDataAccessManager.QueryList<BorrowBookListTable>("select t.ID,t.BORROWID,t.BOOKID " +
                                                                                          "from BorrowBookListTable t " +
                                                                                         $"where t.BORROWID = '{returnBorrowRecord.BORROWID}'");

                foreach (var book in BookList)
                {
                    var re = await _bookManagementServices.UpdateBookStocK(book.BookId, BookStockStatus.Return);
                    if(re.Status == ResultStatus.Failure)
                    {
                        return new ReturnRecord(string.Empty, re.Message, ResultStatus.Failure);
                    }
                }

                borrowDetails.ReturnDate = returnBorrowRecord.RETURNDATE;
                var overDueDate = (borrowDetails.ReturnDate - borrowDetails.DueDate);
                int overDueDays = overDueDate.HasValue ? (int)overDueDate.Value.TotalDays : 0;

                int charge = Convert.ToInt32(_configuration["DueAmmountSetting:PerDayChargeAmmount"]);

                var result = await _dataAccess.UpdateAsync(returnBorrowRecord.BORROWID, borrowDetails);
                await _dataAccess.SaveChangesAsync();
                return new ReturnRecord(new { overDue = overDueDays, penaltyAmount = overDueDays * charge }, "Return Sucessfull", ResultStatus.Success);
            }
            catch (Exception ex)
            {
                return new ReturnRecord(string.Empty, ex.Message + "\n" + ex.InnerException, ResultStatus.Failure);
            }
        }
    }
}
