using ClassRecord.BookManagement;
using DataBaseModels.DBModels;
using EnumClasses;

namespace ModelValidateAndConvert.BookManagement
{
    public class BookManagementGetAll
    {
        private List<BookManagementGetRecord> bookManagementGetRecords = new List<BookManagementGetRecord>();
        private int _bookId;
        private string? _title;
        private string? _author;
        private string? _isbn;
        private string? _category;
        private int _copiesAvailable;
        private int _publishedYear;
        private int _status;

        private int _pageNumber;
        private int _size;

        private string? _errorMessage = string.Empty;
        public BookManagementGetAll(string title, string category, string isbn, int pageNumber, int size)
        {
            _title = title;
            _category = category;
            _isbn = isbn;
            _pageNumber = pageNumber;
            _size = size;
        }

        public bool IsValid()
        {
            if (!string.IsNullOrEmpty(_title))
            {
                if (_title.Contains("/*") ||
                    _title.Contains("*\\") ||
                    _title.Contains("--") ||
                    _title.Contains("- -"))
                {
                    _errorMessage = "Unwanted Title";
                    return false;
                }
            }
            if (!string.IsNullOrEmpty(_category))
            {
                if (_category.Contains("/*") ||
                _category.Contains("*\\") ||
                _category.Contains("--") ||
                _category.Contains("- -"))
                {
                    _errorMessage = "Unwanted Category";
                    return false;
                }
            }
            if (!string.IsNullOrEmpty(_isbn))
            {
                if (_isbn.Contains("/*") ||
                _isbn.Contains("*\\") ||
                _isbn.Contains("--") ||
                _isbn.Contains("- -"))
                {
                    _errorMessage = "Unwanted ISBN";
                    return false;
                }
            }

            if (_pageNumber <= 0)
            {
                _errorMessage = "Page number must be greater than zero.";
                return false;
            }
            else if (_size <= 0)
            {
                _errorMessage = "Size must be greater than zero.";
                return false;
            }
            return true;
        }
        public string? GetErrorMessage()
        {
            return _errorMessage;
        }

        public string GetQuery()
        {
            return "SELECT t.BOOKID, " +
                          "t.TITLE, " +
                          "t.AUTHOR, " +
                          "t.ISBN, " +
                          "t.CATEGORY, " +
                          "t.COPIESAVAILABLE, " +
                          "t.PUBLISHEDYEAR, " +
                          "t.STATUS " +
                          "FROM BookManagementTable t " +
                          "WHERE t.ISDELETE != 1 " +
                         $"AND (t.TITLE LIKE '%{_title}%') " +
                         $"AND (t.CATEGORY LIKE '%{_category}%') " +
                         $"AND (t.ISBN LIKE '%{_isbn}%') " +
                          "ORDER BY t.BOOKID " +
                         $"OFFSET ({_pageNumber} - 1) * {_size} ROWS " +
                         $"FETCH NEXT {_size} ROWS ONLY;";
        }

        public BookManagementGetAll(List<BookManagementTable> getList)
        {
            foreach (var item in getList)
            {
                if(item.IsDelete == (int)DeleteStatus.NotDelete)
                {
                    SetData(item);
                    bookManagementGetRecords.Add(GetObject());
                }
            }
        }

        private void SetData(BookManagementTable data)
        {
            _bookId = data.BookId;
            _title = data.Title;
            _author = data.Author;
            _isbn = data.Isbn;
            _category = data.Category;
            _copiesAvailable = data.CopiesAvailable;
            _publishedYear = data.PublishedYear;
            _status = data.Status;
        }

        public BookManagementGetRecord GetObject()
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

        public List<BookManagementGetRecord> GetData()
        {
            return bookManagementGetRecords;
        }
    }
}
