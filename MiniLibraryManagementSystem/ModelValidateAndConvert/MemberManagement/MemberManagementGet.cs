using ClassRecord.MemberManagement;
using DataBaseModels.DBModels;
using EnumClasses;

namespace ModelValidateAndConvert.MemberManagement
{
    public class MemberManagementGet
    {
        private int _memberId;
        private string _fullName;
        private string _email;
        private string _phone;
        private DateTime _joinDate;
        private int _isActive;

        public MemberManagementGet(MemberManagementTable getObject)
        {
            _memberId = getObject.MemberId;
            _fullName = getObject.FullName;
            _email = getObject.Email;
            _phone = getObject.Phone;
            _joinDate = getObject.JoinDate;
            _isActive = getObject.IsActive;
        }

        public MemberManagementGetRecord GetData()
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
    }
}
