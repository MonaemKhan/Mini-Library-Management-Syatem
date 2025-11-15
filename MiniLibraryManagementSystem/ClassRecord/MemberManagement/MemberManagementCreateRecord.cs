namespace ClassRecord.MemberManagement
{
    public record MemberManagementCreateRecord
    (
        string? FullName,
        string? Email,
        string? Phone,
        DateTime? JoinDate
    );
}
