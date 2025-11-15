using EnumClasses;

namespace ClassRecord.MemberManagement
{
    public record MemberManagementGetRecord
    (
        int MemberId,
        string FullName,
        string Email,
        string Phone,
        DateTime JoinDate,
        ActiveStatus IsActive
    );
}
