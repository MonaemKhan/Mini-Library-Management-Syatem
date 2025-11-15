using ClassRecord.MemberManagement;
using DataBaseModels.DBModels;
using EnumClasses;
using System.Text.RegularExpressions;

namespace ModelValidateAndConvert.MemberManagement
{
    public class MemberManagementUpdate
    {
        private int _memberId;
        private string? _fullName;
        private string? _email;
        private string? _phone;
        private DateTime? _joinDate;
        private int _isActive;

        private string? _errorMessage = string.Empty;
        public MemberManagementUpdate(MemberManagementUpdateRecord recordData)
        {
            _memberId = recordData.MemberId;
            _fullName = recordData.FullName;
            _email = recordData.Email;
            _phone = recordData.Phone;
            _joinDate = recordData.JoinDate;
            _isActive = recordData.IsActive;
        }

        public bool IsValid(int Id)
        {
            if (_memberId <= 0 || _memberId == null)
            {
                _errorMessage = "Member Id not found for update.";
                return false;
            }
            if (Id != _memberId)
            {
                _errorMessage = "Record MisMatch.";
                return false;
            }
            else if (string.IsNullOrEmpty(_fullName))
            {
                _errorMessage = "Full name cannot be null or empty.";
                return false;
            }
            else if (!Regex.IsMatch(_fullName, @"^[a-zA-Z\s\.]+$"))
            {
                _errorMessage = "Name only contain A-Z and dot(.).";
                return false;
            }
            else if (string.IsNullOrEmpty(_email))
            {
                _errorMessage = "Email cannot be null or empty.";
                return false;
            }
            else if (!_email.Contains("@")
                || !_email.Contains(".com"))
            {
                _errorMessage = "Not a Valid Email.";
                return false;
            }
            else if (string.IsNullOrEmpty(_phone))
            {
                _errorMessage = "Phone no. cannot be null or empty.";
                return false;
            }
            else if (_phone.Length < 11)
            {
                _errorMessage = "Invalid Phone Number.";
                return false;
            }
            else if (_phone.Length < 13 && _phone.Length != 11)
            {
                _errorMessage = "Invalid Phone Number.";
                return false;
            }
            else
            {
                try
                {
                    _joinDate = _joinDate ?? DateTime.Now;
                    var date = Convert.ToDateTime(_joinDate);
                }
                catch
                {
                    _errorMessage = "Join Date is not valid.";
                    return false;
                }
            }
            return true;
        }

        public string GetErrorMessage()
        {
            return _errorMessage ?? string.Empty;
        }

        public MemberManagementTable GetData()
        {
            var data = new MemberManagementTable
            {
                MemberId = _memberId,
                FullName = _fullName!,
                Email = _email!,
                Phone = (_phone.Length == 11) ? "+88" + _phone : _phone,
                JoinDate = _joinDate ?? DateTime.Now,
                IsActive = (_isActive == null) ? (int)ActiveStatus.Active : _isActive
            };
            return data;
        }
    }
}
