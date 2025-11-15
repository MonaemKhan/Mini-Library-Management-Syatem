using ClassRecord;
using ClassRecord.BorrowingManagement;
using DataAccessManager;
using DataBaseModels.DBModels;
using EnumClasses;
using ModelValidateAndConvert.BorrowingManagement;

namespace ServiceManager.BorrowingManagement
{
    public interface IBorrowBookListServices
    {
        public Task<ReturnRecord> BorrowBookListCreate(BorrowBookListCreateRecord record, int BorrowId);
        public Task SaveChanges();
    }
    public class BorrowBookListServices : IBorrowBookListServices
    {
        private readonly IEFCoreDataAccessManager<BorrowBookListTable> _dataAccess;
        public BorrowBookListServices(IEFCoreDataAccessManager<BorrowBookListTable> dataAccess)
        {
            _dataAccess = dataAccess;
        }
        public async Task<ReturnRecord> BorrowBookListCreate(BorrowBookListCreateRecord record,int BorrowId)
        {
            try
            {
                BorrowManagementBookListCreate obj = new BorrowManagementBookListCreate(record);
                var result = await _dataAccess.InsertAsync(obj.GetData(BorrowId));
                return new ReturnRecord(result.BookId, "Create Sucessfull", ResultStatus.Success);
            }
            catch (Exception ex)
            {
                return new ReturnRecord(string.Empty, ex.Message + "\n" + ex.InnerException, ResultStatus.Failure);
            }
        }
        public async Task SaveChanges()
        {
            await _dataAccess.SaveChangesAsync();
        }
    }
}
