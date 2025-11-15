
using ClassRecord.BorrowingManagement;
using DataBaseModels.DBModels;
using EnumClasses;

namespace ModelValidateAndConvert.BorrowingManagement
{
    public class BorrowManagementDetailsCreate
    {
        private int _memberId;
        private DateTime _borrowDate;
        private DateTime _dueDate;

        private string _errorMessage = string.Empty;
        public BorrowManagementDetailsCreate(BorrowDetailsCreateRecord record)
        {
            _memberId = record.MEMBERID;
            _borrowDate = record.BORROWDATE;
            _dueDate = record.DUEDATE;
        }

        public bool IsValid()
        {
            if (_memberId <= 0)
            {
                _errorMessage = "Invalid Member ID";
                return false;
            }
            else if (_borrowDate == default(DateTime))
            {
                _errorMessage = "Invalid Borrow Date";
                return false;
            }
            else if (_dueDate == default(DateTime))
            {
                _errorMessage = "Invalid Due Date";
                return false;
            }
            return true;
        }

        public string GetErrorMessage()
        {
            return _errorMessage;
        }

        public BorrowDetailsTable GetData()
        {
            return new BorrowDetailsTable
            {
                MemberId = _memberId,
                BorrowDate = _borrowDate,
                DueDate = _dueDate,
                IsDelete = (int)DeleteStatus.NotDelete
            };
        }
    }
}
