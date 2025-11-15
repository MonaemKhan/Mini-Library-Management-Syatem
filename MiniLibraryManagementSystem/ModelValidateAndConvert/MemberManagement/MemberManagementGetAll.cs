using ClassRecord.MemberManagement;
using DataBaseModels.DBModels;
using EnumClasses;

namespace ModelValidateAndConvert.MemberManagement
{
    public class MemberManagementGetAll
    {
        private List<MemberManagementGetRecord> memberManagementGetRecords = new List<MemberManagementGetRecord>();
        private int _memberId;
        private string _fullName;
        private string _email;
        private string _phone;
        private DateTime _joinDate;
        private int _isActive;

        private int _pageNumber;
        private int _size;

        private string? _errorMessage = string.Empty;
        public MemberManagementGetAll(string fullName, string email, string phone, int pageNumber, int size)
        {
            _fullName = fullName;
            _email = email;
            _phone = phone;
            _pageNumber = pageNumber;
            _size = size;
        }

        public bool IsValid()
        {
            if (!string.IsNullOrEmpty(_fullName))
            {
                if (_fullName.Contains("/*") ||
                    _fullName.Contains("*\\") ||
                    _fullName.Contains("--") ||
                    _fullName.Contains("- -"))
                {
                    _errorMessage = "Unwanted Fulll Name";
                    return false;
                }
            }
            if (!string.IsNullOrEmpty(_email))
            {
                if (_email.Contains("/*") ||
                _email.Contains("*\\") ||
                _email.Contains("--") ||
                _email.Contains("- -"))
                {
                    _errorMessage = "Unwanted Email";
                    return false;
                }
            }
            if (!string.IsNullOrEmpty(_phone))
            {
                if (_phone.Contains("/*") ||
                _phone.Contains("*\\") ||
                _phone.Contains("--") ||
                _phone.Contains("- -"))
                {
                    _errorMessage = "Unwanted Phone Number";
                    return false;
                }
            }

            if (_pageNumber <= 0)
            {
                _errorMessage = "Page number must be greater than zero.";
                return false;
            }
            else if (_size <= 0)
            {
                _errorMessage = "Size must be greater than zero.";
                return false;
            }
            return true;
        }
        public string? GetErrorMessage()
        {
            return _errorMessage;
        }

        public string GetQuery()
        {
            return "SELECT t.MEMBERID, " +
                          "t.FULLNAME, " +
                          "t.EMAIL, " +
                          "t.PHONE, " +
                          "t.JOINDATE, " +
                          "t.ISACTIVE " +
                          "FROM MemberManagementTable t " +
                          "WHERE t.ISDELETE != 1 " +
                         $"AND (t.FULLNAME LIKE '%{_fullName}%') " +
                         $"AND (t.EMAIL LIKE '%{_email}%') " +
                         $"AND (t.PHONE LIKE '%{_phone}%') " +
                          "ORDER BY t.MEMBERID " +
                         $"OFFSET ({_pageNumber} - 1) * {_size} ROWS " +
                         $"FETCH NEXT {_size} ROWS ONLY;";
        }

        public MemberManagementGetAll(List<MemberManagementTable> getList)
        {
            foreach (var item in getList)
            {
                if (item.IsDelete == (int)DeleteStatus.NotDelete)
                {
                    SetData(item);
                    memberManagementGetRecords.Add(GetObject());
                }
            }
        }

        private void SetData(MemberManagementTable data)
        {
            _memberId = data.MemberId;
            _fullName = data.FullName;
            _email = data.Email;
            _phone = data.Phone;
            _joinDate = data.JoinDate;
            _isActive = data.IsActive;
        }

        public MemberManagementGetRecord GetObject()
        {
            return new MemberManagementGetRecord
            (
                MemberId: _memberId,
                FullName: _fullName,
                Email: _email,
                Phone: _phone,
                JoinDate: _joinDate,
                IsActive: (_isActive == 0) ? ActiveStatus.InActive : ActiveStatus.Active
            );
        }

        public List<MemberManagementGetRecord> GetData()
        {
            return memberManagementGetRecords;
        }
    }
}
