using DataBaseModels.DBModels;
using EnumClasses;

namespace ModelValidateAndConvert.BookManagement
{
    public class BookManagementUpdateBookList
    {
        private int _bookId;
        private string? _title;
        private string? _author;
        private string? _isbn;
        private string? _category;
        private int _copiesAvailable;
        private int _publishedYear;
        private BookStockStatus _bookStockStatus;

        private string? _errorMessage = string.Empty;
        public BookManagementUpdateBookList(BookManagementTable data, BookStockStatus bookStockStatus)
        {
            _bookId = data.BookId;
            _title = data.Title;
            _author = data.Author;
            _isbn = data.Isbn;
            _category = data.Category;
            if (bookStockStatus == BookStockStatus.Return)
                _copiesAvailable = data.CopiesAvailable + 1;
            else
                _copiesAvailable = data.CopiesAvailable - 1;
            _publishedYear = data.PublishedYear;
            _bookStockStatus = bookStockStatus;
        }

        public bool IsValid()
        {
            if (_copiesAvailable < 0)
            {
                _errorMessage = "Book coppies is not available.";
                return false;
            }
            return true;
        }

        public string GetErrorMessage()
        {
            return _errorMessage ?? string.Empty;
        }

        public BookManagementTable GetData()
        {
            var data = new BookManagementTable
            {
                BookId = _bookId,
                Title = _title!,
                Author = _author!,
                Isbn = _isbn!,
                Category = _category,
                CopiesAvailable = _copiesAvailable,
                PublishedYear = _publishedYear,
                Status = _copiesAvailable > 0 ? (int)AvailabityStatus.Available : (int)AvailabityStatus.NotAvailable
            };
            return data;
        }
    }
}
