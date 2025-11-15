using ClassRecord;
using ConfigureManager;
using EnumClasses;
using Microsoft.AspNetCore.Mvc;

namespace MiniLibraryManagementSystem.Controllers.ReturnManagement
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ReturningBookController : ControllerBase
    {
        private readonly IRepoManger _repoManger;
        public ReturningBookController(IRepoManger repoManger)
        {
            _repoManger = repoManger;
        }

        [HttpPost]
        public async Task<IActionResult> ReturnBorrowBook([FromBody] ClassRecord.ReturnManagement.ReturnBorrowedBookRecord returnBorrowRecord)
        {
            try
            {
                var result = await _repoManger.ReturnManagementServices.ReturnBorrowBook(returnBorrowRecord);
                if (result.Status == ResultStatus.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ReturnRecord(string.Empty, ex.Message + "\n" + ex.InnerException, ResultStatus.Failure));
            }
        }
    }
}
