using ClassRecord;
using ClassRecord.BookManagement;
using ConfigureManager;
using EnumClasses;
using Microsoft.AspNetCore.Mvc;

namespace MiniLibraryManagementSystem.Controllers.BookManagement
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class BookManagementController : ControllerBase
    {
        private readonly IRepoManger _repoManger;
        public BookManagementController(IRepoManger repoManger)
        {
            _repoManger = repoManger;
        }

        [HttpGet("All")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _repoManger.BookManagementServices.GetAllBooks();
                if (result.Status == ResultStatus.Success)
                    return Ok(result);
                else
                    return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ReturnRecord("", ex.Message, ResultStatus.Failure));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get(int pageNumber = 1, int size = 5, string title = null, string category = null, string isbn = null)
        {
            try
            {
                var result = await _repoManger.BookManagementServices.GetAllBooks(pageNumber, size, title, category, isbn);
                if (result.Status == ResultStatus.Success)
                    return Ok(result);
                else
                    return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ReturnRecord("", ex.Message, ResultStatus.Failure));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var result = await _repoManger.BookManagementServices.GetBookById(id);
                if (result.Status == ResultStatus.Success)
                    return Ok(result);
                else
                    return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ReturnRecord("", ex.Message, ResultStatus.Failure));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(BookManagementCreateRecord bookManagementTable)
        {
            try
            {
                var result = await _repoManger.BookManagementServices.CreateBook(bookManagementTable);
                if (result.Status == ResultStatus.Success)
                    return Ok(result);
                else
                    return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ReturnRecord("", ex.Message, ResultStatus.Failure));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, BookManagementUpdateRecord bookManagementTable)
        {
            try
            {
                var result = await _repoManger.BookManagementServices.UpdateBook(id, bookManagementTable);
                if (result.Status == ResultStatus.Success)
                    return Ok(result);
                else
                    return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ReturnRecord("", ex.Message, ResultStatus.Failure));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _repoManger.BookManagementServices.DeleteBook(id);
                if (result.Status == ResultStatus.Success)
                    return Ok(result);
                else
                    return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ReturnRecord("", ex.Message, ResultStatus.Failure));
            }
        }
    }
}
