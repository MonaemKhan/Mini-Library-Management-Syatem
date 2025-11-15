using ClassRecord;
using ClassRecord.MemberManagement;
using ConfigureManager;
using EnumClasses;
using Microsoft.AspNetCore.Mvc;

namespace MiniLibraryManagementSystem.Controllers.MemberManagement
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class MemberManagementController : ControllerBase
    {
        private readonly IRepoManger _repoManger;
        public MemberManagementController(IRepoManger repoManger)
        {
            _repoManger = repoManger;
        }

        [HttpGet("All")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _repoManger.MemberManagementServices.GetAllMembers();
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
        public async Task<IActionResult> Get(int pageNumber = 1, int size = 5, string fullanme = null, string email = null, string phone = null)
        {
            try
            {
                var result = await _repoManger.MemberManagementServices.GetAllMembers(pageNumber, size, fullanme, email, phone);
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
                var result = await _repoManger.MemberManagementServices.GetMemberById(id);
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
        public async Task<IActionResult> Post(MemberManagementCreateRecord MemberManagementTable)
        {
            try
            {
                var result = await _repoManger.MemberManagementServices.CreateMember(MemberManagementTable);
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
        public async Task<IActionResult> Put(int id, MemberManagementUpdateRecord MemberManagementTable)
        {
            try
            {
                var result = await _repoManger.MemberManagementServices.UpdateMember(id, MemberManagementTable);
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
                var result = await _repoManger.MemberManagementServices.DeleteMember(id);
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
