using ClassRecord;
using ClassRecord.BookManagement;
using DataAccessManager;
using DataBaseModels.DBModels;
using EnumClasses;
using ModelValidateAndConvert.BookManagement;

namespace ServiceManager.BookManagement
{
    public interface IBookManagementServices
    {
        public Task<ReturnRecord> CreateBook(BookManagementCreateRecord book);
        public Task<ReturnRecord> UpdateBook(int Id, BookManagementUpdateRecord book);
        public Task<ReturnRecord> DeleteBook(int Id);
        public Task<ReturnRecord> GetBookById(int Id);
        public Task<ReturnRecord> GetAllBooks(int pageNumber, int size, string title, string category, string isbn);
        public Task<ReturnRecord> GetAllBooks();
        public Task<ReturnRecord> UpdateBookStocK(int Id , BookStockStatus bookStockStatus);
        public Task SaveChanges();
    }
    public class BookManagementServices : IBookManagementServices
    {
        private readonly IEFCoreDataAccessManager<BookManagementTable> _dataAccess;
        public BookManagementServices(IEFCoreDataAccessManager<BookManagementTable> dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task<ReturnRecord> CreateBook(BookManagementCreateRecord bookRecord)
        {
            try
            {
                BookManagementCreate obj = new BookManagementCreate(bookRecord);
                if (!obj.IsValid())
                {
                    return new ReturnRecord(string.Empty, obj.GetErrorMessage(), ResultStatus.Failure);
                }
                var result = await _dataAccess.InsertAsync(obj.GetData());
                await _dataAccess.SaveChangesAsync();
                return new ReturnRecord(result.BookId, "Create Sucessfull", ResultStatus.Success);
            }
            catch (Exception ex)
            {
                return new ReturnRecord(string.Empty, ex.Message + "\n" + ex.InnerException, ResultStatus.Failure);
            }
        }

        public async Task<ReturnRecord> UpdateBook(int Id, BookManagementUpdateRecord book)
        {
            try
            {
                BookManagementUpdate obj = new BookManagementUpdate(book);
                if (!obj.IsValid(Id))
                {
                    return new ReturnRecord(string.Empty, obj.GetErrorMessage(), ResultStatus.Failure);
                }
                var bookData = await _dataAccess.GetByIdAsync(Id);
                if (bookData == null
                    || bookData.IsDelete == (int)DeleteStatus.Delete)
                {
                    return new ReturnRecord(string.Empty, "Book Not Found", ResultStatus.Failure);
                }
                var result = await _dataAccess.UpdateAsync(Id, obj.GetData());
                await _dataAccess.SaveChangesAsync();
                return new ReturnRecord(result.BookId, "Update Sucessfull", ResultStatus.Success);
            }
            catch (Exception ex)
            {
                return new ReturnRecord(string.Empty, ex.Message + "\n" + ex.InnerException, ResultStatus.Failure);
            }
        }

        public async Task<ReturnRecord> DeleteBook(int Id)
        {
            try
            {
                var book = await _dataAccess.GetByIdAsync(Id);
                if (book == null || book.IsDelete == (int)DeleteStatus.Delete)
                {
                    return new ReturnRecord(string.Empty, "Book Not Found", ResultStatus.Failure);
                }   
                book.IsDelete = (int)DeleteStatus.Delete;
                var result = await _dataAccess.DeleteAsync(Id,book);
                await _dataAccess.SaveChangesAsync();
                if(result != null)
                    return new ReturnRecord(Id, "Delete Sucessfull", ResultStatus.Success);
                else
                    return new ReturnRecord(string.Empty, "Book Not Found", ResultStatus.Failure);
            }
            catch (Exception ex)
            {
                return new ReturnRecord(string.Empty, ex.Message + "\n" + ex.InnerException, ResultStatus.Failure);
            }
        }

        public async Task<ReturnRecord> GetBookById(int Id)
        {
            try
            {
                var result = await _dataAccess.GetByIdAsync(Id);
                if (result == null || result.IsDelete == (int)DeleteStatus.Delete)
                {
                    return new ReturnRecord(string.Empty, "Book Not Found", ResultStatus.Failure);
                }
                return new ReturnRecord(new BookManagementGet(result).GetData(), "Get Sucessfull", ResultStatus.Success);
            }
            catch (Exception ex)
            {
                return new ReturnRecord(string.Empty, ex.Message + "\n" + ex.InnerException, ResultStatus.Failure);
            }
        }

        public async Task<ReturnRecord> GetAllBooks(int pageNumber,int size,string title,string category,string isbn)
        {
            try
            {                
                var obj = new BookManagementGetAll(title, category, isbn, pageNumber, size);
                if (!obj.IsValid())
                {
                    return new ReturnRecord(string.Empty, obj.GetErrorMessage(), ResultStatus.Failure);
                }
                var result = await DapperDataAccessManager.QueryList<BookManagementTable>(obj.GetQuery());
                if (result == null)
                {
                    return new ReturnRecord(string.Empty, "Book Not Found", ResultStatus.Failure);
                }
                return new ReturnRecord(new BookManagementGetAll(result).GetData(), "Get All Sucessfull", ResultStatus.Success);
            }
            catch (Exception ex)
            {
                return new ReturnRecord(string.Empty, ex.Message + "\n" + ex.InnerException, ResultStatus.Failure);
            }
        }

        public async Task<ReturnRecord> GetAllBooks()
        {
            try
            {
                var result = await _dataAccess.GetAllAsync();
                if (result == null)
                {
                    return new ReturnRecord(string.Empty, "Book Not Found", ResultStatus.Failure);
                }
                return new ReturnRecord(new BookManagementGetAll(result).GetData(), "Get All Sucessfull", ResultStatus.Success);
            }
            catch (Exception ex)
            {
                return new ReturnRecord(string.Empty, ex.Message + "\n" + ex.InnerException, ResultStatus.Failure);
            }
        }

        public async Task<ReturnRecord> UpdateBookStocK(int Id , BookStockStatus bookStockStatus)
        {
            try
            {
                var book = await _dataAccess.GetByIdAsync(Id);
                if (book == null || book.IsDelete == (int)DeleteStatus.Delete)
                {
                    return new ReturnRecord(string.Empty, "Book Not Found", ResultStatus.Failure);
                }
                BookManagementUpdateBookList obj = new BookManagementUpdateBookList(book, bookStockStatus);
                if (!obj.IsValid())
                {
                    return new ReturnRecord(string.Empty, obj.GetErrorMessage(), ResultStatus.Failure);
                }
                var result = await _dataAccess.UpdateAsync(Id, obj.GetData());
                return new ReturnRecord(result.BookId, "Update Sucessfull", ResultStatus.Success);
            }
            catch (Exception ex)
            {
                return new ReturnRecord(string.Empty, ex.Message + "\n" + ex.InnerException, ResultStatus.Failure);
            }
        }

        public async Task SaveChanges()
        {
            await _dataAccess.SaveChangesAsync();
        }
    }
}
