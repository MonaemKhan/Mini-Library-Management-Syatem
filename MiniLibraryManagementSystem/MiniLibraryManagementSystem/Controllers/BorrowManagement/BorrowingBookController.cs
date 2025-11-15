using ClassRecord.BorrowingManagement;
using ConfigureManager;
using EnumClasses;
using Microsoft.AspNetCore.Mvc;

namespace MiniLibraryManagementSystem.Controllers.BorrowManagement
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class BorrowingBookController : ControllerBase
    {
        private readonly IRepoManger _repoManger;

        public BorrowingBookController(IRepoManger repoManger)
        {
            _repoManger = repoManger;
        }

        [HttpPost]
        public async Task<IActionResult> Post(BorrowDetailsCreateRecord record)
        {
            var result = await _repoManger.BorrowDetailsServices.BorrowDetailsCreate(record);
            if (result.Status == ResultStatus.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}
