using Microsoft.AspNetCore.Mvc;
using TimeIsMoney.Api.Attributes;
using TimeIsMoney.Common;
using TimeIsMoney.Services.Auth;
using TimeIsMoney.Services.Auth.Models;

namespace TimeIsMoney.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly DomainTaskStatus _taskStatus;

        public AuthController(IAuthService authService, DomainTaskStatus taskStatus)
        {
            _authService = authService;
            _taskStatus = taskStatus;
        }

        [HttpPost]
        [Route("register")]
        [ServiceFilter(typeof(ValidateModelAttribute))]
        public IActionResult Register(UserRegistrationModel userRegistration)
        {
            _authService.Register(userRegistration);
            if (!_taskStatus.HasErrors)
            {
                return Ok();
            }
            return BadRequest(_taskStatus.ErrorCollection);
        }
    }
}