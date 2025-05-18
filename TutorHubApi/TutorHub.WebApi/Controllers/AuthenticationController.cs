using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TutorHub.BusinessLogic.IServices.IUserServices;
using TutorHub.BusinessLogic.Models.User;

namespace TutorHub.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController(IAuthService authService) : ControllerBase
    {
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    return this.BadRequest("Invalid payload");
                }

                var (status, token, userName) = await authService.Login(model);
                if (status == 0)
                {
                    return this.BadRequest(token);
                }

                return this.Ok(new { token, userName });
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
