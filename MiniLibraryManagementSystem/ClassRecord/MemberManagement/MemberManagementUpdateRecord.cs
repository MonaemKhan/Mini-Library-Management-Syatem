namespace ClassRecord.MemberManagement
{
    public record MemberManagementUpdateRecord
    (
        int MemberId,
        string FullName,
        string Email,
        string Phone,
        DateTime JoinDate,
        int IsActive
    );
}
