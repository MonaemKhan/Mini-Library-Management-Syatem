using ClassRecord;
using ConfigureManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MiniLibraryManagementSystem.Controllers.Login
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class LoginController : ControllerBase
    {
        private readonly IRepoManger _repoManger;
        public LoginController(IRepoManger repoManger)
        {
            _repoManger = repoManger;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Post(LoginUserRecord user)
        {
            try
            {
                var result = _repoManger.LoginService.AuthenticateUserAsync(user);
                if (result.Status == EnumClasses.ResultStatus.Success)
                    return Ok(result);
                else
                    return Unauthorized(result);
            }
            catch (Exception ex)
            {
                {
                    return Unauthorized(BadRequest(new ReturnRecord("", ex.Message, EnumClasses.ResultStatus.Failure)));
                }
            }
        }
    }
}
