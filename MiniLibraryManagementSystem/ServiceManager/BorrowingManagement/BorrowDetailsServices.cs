using ClassRecord;
using ClassRecord.BorrowingManagement;
using DataAccessManager;
using DataBaseModels.DBModels;
using EnumClasses;
using ModelValidateAndConvert.BorrowingManagement;
using ServiceManager.BookManagement;
using ServiceManager.MemberManagement;

namespace ServiceManager.BorrowingManagement
{
    public interface IBorrowDetailsServices
    {
        public Task<ReturnRecord> BorrowDetailsCreate(BorrowDetailsCreateRecord record);
    }
    public class BorrowDetailsServices : IBorrowDetailsServices
    {
        private readonly IEFCoreDataAccessManager<BorrowDetailsTable> _dataAccess;
        private readonly IMemberManagementServices _memberManagementServices;
        private readonly IBookManagementServices _bookManagementServices;
        private readonly IBorrowBookListServices _borrowBookListServices;

        public BorrowDetailsServices(IEFCoreDataAccessManager<BorrowDetailsTable> dataAccess,
                                     IMemberManagementServices memberManagementServices,
                                     IBookManagementServices bookManagementServices,
                                     IBorrowBookListServices borrowBookListServices)
        {
            _dataAccess = dataAccess;
            _memberManagementServices = memberManagementServices;
            _bookManagementServices = bookManagementServices;
            _borrowBookListServices = borrowBookListServices;
        }

        public async Task<ReturnRecord> BorrowDetailsCreate(BorrowDetailsCreateRecord record)
        {
            try
            {
                await _dataAccess.Transition();
                var validateModel = new BorrowManagementDetailsCreate(record);
                if (!validateModel.IsValid())
                {
                    return new ReturnRecord(string.Empty, validateModel.GetErrorMessage(), ResultStatus.Failure);
                }
                var member = await _memberManagementServices.GetMemberById(record.MEMBERID);
                if (member.Status == ResultStatus.Failure)
                {
                    return new ReturnRecord(string.Empty,member.Message,ResultStatus.Failure);
                }

                if(await TotalBorrowedByMember(record.MEMBERID) >= 5)
                {
                    return new ReturnRecord(string.Empty, "This member already exceed the borrowing limit", ResultStatus.Failure);
                }

                var borrower = await _dataAccess.InsertAsync(validateModel.GetData());
                await _dataAccess.SaveChangesAsync();

                foreach (var book in record.BORROWBOOKLIST)
                {
                    if (await isBookPreeviouslyBorrow(record.MEMBERID, book.BOOKID))
                    {
                        await _dataAccess.RollbackAsync();
                        return new ReturnRecord(string.Empty, $"Member already borrowed the book with BOOKID - {book.BOOKID}", ResultStatus.Failure);
                    }

                    var result  = await _bookManagementServices.UpdateBookStocK(book.BOOKID, BookStockStatus.Borrow);
                    if(result.Status == ResultStatus.Failure)
                    {
                        await _dataAccess.RollbackAsync();
                        return new ReturnRecord(string.Empty, result.Message, ResultStatus.Failure);
                    }
                    result = await _borrowBookListServices.BorrowBookListCreate(book, borrower.BorrowId);
                    if(result.Status == ResultStatus.Failure)
                    {
                        await _dataAccess.RollbackAsync();
                        return new ReturnRecord(string.Empty, result.Message, ResultStatus.Failure);
                    }
                }

                await _dataAccess.SaveChangesAsync();

                await _dataAccess.CommitAsync();

                await _dataAccess.DisposeAsync();

                return new ReturnRecord(borrower.BorrowId, "Borrow Details Created Successfully", ResultStatus.Success);

            }
            catch (Exception ex)
            {
                return new ReturnRecord(string.Empty, ex.Message + "\n" + ex.InnerException, ResultStatus.Failure);
            }
        }

        private async Task<int> TotalBorrowedByMember(int memberId)
        {
            var result = await DapperDataAccessManager.QueryObject<DataCountRecord>($"select count(*)  countNum from BorrowDetailsTable t where t.MEMBERID = '{memberId}' and t.RETURNDATE is null and t.ISDELETE = 0");
            return result.countNum;
        }

        private async Task<bool> isBookPreeviouslyBorrow(int memberId, int bookId)
        {
            var result = await DapperDataAccessManager.QueryObject<DataCountRecord>($"select count(*) as countNum " +
                                                                                     "from BorrowDetailsTable t " +
                                                                                     "left join BorrowBookListTable tt " +
                                                                                     "on t.BORROWID = tt.BORROWID " +
                                                                                     "where t.RETURNDATE is null " +
                                                                                     "and t.ISDELETE = 0 " +
                                                                                     "and tt.ISDELETE = 0 " +
                                                                                    $"and tt.BOOKID = '{bookId}' " +
                                                                                    $"and t.MEMBERID = '{memberId}'");
            if(result.countNum > 0)
            {
                return true;
            }
            return false;
        }
    }
}
