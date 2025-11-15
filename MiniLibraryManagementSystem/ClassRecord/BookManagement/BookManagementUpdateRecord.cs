namespace ClassRecord.BookManagement
{
    public record BookManagementUpdateRecord
    (
        int BookId,
        string Title,
        string Author,
        string Isbn,
        string Category,
        int CopiesAvailable,
        int PublishedYear
    );
}
