
using ClassRecord.ReturnManagement;

namespace ModelValidateAndConvert.ReturnManagement
{
    public class ReturnManagementBorrowDetailsUpdate
    {
        private int _borrowId;
        private DateTime _returnDate;

        private string _errorMessage = string.Empty;

        public ReturnManagementBorrowDetailsUpdate(ReturnBorrowedBookRecord record)
        {
            _borrowId = record.BORROWID;
            _returnDate = record.RETURNDATE;
        }

        public bool IsValid()
        {
            if (_borrowId <= 0)
            {
                _errorMessage = "Invalid Borrow ID";
                return false;
            }
            else if (_returnDate == default(DateTime))
            {
                _errorMessage = "Invalid Return Date";
                return false;
            }
            return true;
        }

        public string GetErrorMessage()
        {
            return _errorMessage;
        }
    }
}
