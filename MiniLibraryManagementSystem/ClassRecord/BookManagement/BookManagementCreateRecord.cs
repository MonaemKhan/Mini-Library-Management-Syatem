namespace ClassRecord.BookManagement
{
    public record BookManagementCreateRecord
    (
        string Title,
        string Author,
        string Isbn,
        string Category,
        int CopiesAvailable,
        int PublishedYear
    );
}
