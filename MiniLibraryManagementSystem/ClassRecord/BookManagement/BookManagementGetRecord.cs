using EnumClasses;

namespace ClassRecord.BookManagement
{
    public record BookManagementGetRecord
    (
        int BookId,
        string Title,
        string Author,
        string Isbn,
        string Category,
        int CopiesAvailable,
        int PublishedYear,
        AvailabityStatus Status
    );
}
