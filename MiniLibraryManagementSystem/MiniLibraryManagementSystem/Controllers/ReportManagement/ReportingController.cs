using ClassRecord;
using ConfigureManager;
using EnumClasses;
using Microsoft.AspNetCore.Mvc;

namespace MiniLibraryManagementSystem.Controllers.ReportManagement
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ReportingController : ControllerBase
    {
        public readonly IRepoManger _repoManger;

        public ReportingController(IRepoManger repoManger)
        {
            _repoManger = repoManger;
        }

        [HttpGet]
        public async Task<IActionResult> GenerateReport(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var result = await _repoManger.ReportManagementServices.GenerateReport(fromDate, toDate);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ReturnRecord("", ex.Message, ResultStatus.Failure));
            }
        }

        [HttpGet("DueNotificationLog")]
        public async Task<IActionResult> GetDueNotificationLog()
        {
            try
            {
                var result = await _repoManger.EmailLogMailManagementServices.GetAllEmailLog();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ReturnRecord("", ex.Message, ResultStatus.Failure));
            }
        }
    }
}
