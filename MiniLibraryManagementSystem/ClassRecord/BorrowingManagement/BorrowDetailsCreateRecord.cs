namespace ClassRecord.BorrowingManagement
{
    public record BorrowDetailsCreateRecord
        (
            int MEMBERID,
            DateTime BORROWDATE,
            DateTime DUEDATE,
            List<BorrowBookListCreateRecord> BORROWBOOKLIST
        );
}
