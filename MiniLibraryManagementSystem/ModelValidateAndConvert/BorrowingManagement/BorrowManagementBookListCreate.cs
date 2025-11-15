using ClassRecord.BorrowingManagement;
using DataBaseModels.DBModels;
using EnumClasses;

namespace ModelValidateAndConvert.BorrowingManagement
{
    public class BorrowManagementBookListCreate
    {
        private int _bookId;

        private string _errorMessage = string.Empty;
        public BorrowManagementBookListCreate(BorrowBookListCreateRecord record)
        {
            _bookId = record.BOOKID;
        }

        public BorrowBookListTable GetData(int Id)
        {
            return new BorrowBookListTable
            {
                BookId = _bookId,
                BorrowId = Id,
                IsDelete = (int)DeleteStatus.NotDelete
            };

        }
    }
}
