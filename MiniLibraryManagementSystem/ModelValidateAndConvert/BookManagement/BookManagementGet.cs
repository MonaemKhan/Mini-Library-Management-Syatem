using ClassRecord.BookManagement;
using DataBaseModels.DBModels;
using EnumClasses;

namespace ModelValidateAndConvert.BookManagement
{
    public class BookManagementGet
    {
        private int _bookId;
        private string? _title;
        private string? _author;
        private string? _isbn;
        private string? _category;
        private int _copiesAvailable;
        private int _publishedYear;
        private int _status;

        public BookManagementGet(BookManagementTable getObject)
        {
            _bookId = getObject.BookId;
            _title = getObject.Title;
            _author = getObject.Author;
            _isbn = getObject.Isbn;
            _category = getObject.Category;
            _copiesAvailable = getObject.CopiesAvailable;
            _publishedYear = getObject.PublishedYear;
            _status = getObject.Status;
        }

        public BookManagementGetRecord GetData()
        {
            return new BookManagementGetRecord
            (
                BookId: _bookId,
                Title: _title!,
                Author: _author!,
                Isbn: _isbn!,
                Category: _category!,
                CopiesAvailable: _copiesAvailable,
                PublishedYear: _publishedYear,
                Status: (_status == 1) ? AvailabityStatus.Available : AvailabityStatus.NotAvailable
            );
        }
    }
}
