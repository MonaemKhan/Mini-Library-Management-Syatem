using ClassRecord.BookManagement;
using DataBaseModels.DBModels;
using EnumClasses;

namespace ModelValidateAndConvert.BookManagement
{
    public class BookManagementCreate
    {
        private string? _title;
        private string? _author;
        private string? _isbn;
        private string? _category;
        private int _copiesAvailable;
        private int _publishedYear;

        private string? _errorMessage = string.Empty;
        public BookManagementCreate(BookManagementCreateRecord recordData)
        {
            _title = recordData.Title;
            _author = recordData.Author;
            _isbn = recordData.Isbn;
            _category = recordData.Category;
            _copiesAvailable = recordData.CopiesAvailable;
            _publishedYear = recordData.PublishedYear;
        }

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(_title))
            {
                _errorMessage = "Title cannot be null or empty.";
                return false;
            }
            else if (string.IsNullOrEmpty(_author))
            {
                _errorMessage = "Author cannot be null or empty.";
                return false;
            }
            else if (string.IsNullOrEmpty(_isbn))
            {
                _errorMessage = "Isbn cannot be null or empty.";
                return false;
            }
            else if (_publishedYear <= 0)
            {
                _errorMessage = "Published Year must be a positive integer.";
                return false;
            }
            else if (_copiesAvailable < 0)
            {
                _errorMessage = "Copies Available cannot be negative.";
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
                Title = _title!,
                Author = _author!,
                Isbn = _isbn!,
                Category = _category,
                CopiesAvailable = _copiesAvailable,
                PublishedYear = _publishedYear,
                Status = _copiesAvailable > 0 ? (int)AvailabityStatus.Available : (int)AvailabityStatus.NotAvailable,
            };
            return data;
        }
    }
}
