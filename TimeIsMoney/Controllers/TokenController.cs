using Microsoft.AspNetCore.Mvc;
using TimeIsMoney.Api.Attributes;
using TimeIsMoney.Common;
using TimeIsMoney.Services.Auth.Models;
using TimeIsMoney.Services.Token;

namespace TimeIsMoney.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/token")]
    public class TokenController : Controller
    {
        private readonly ITokenService _tokenService;
        private readonly DomainTaskStatus _taskStatus;

        public TokenController(ITokenService tokenService, DomainTaskStatus taskStatus)
        {
            _tokenService = tokenService;
            _taskStatus = taskStatus;
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidateModelAttribute))]
        public IActionResult Token([FromBody] LoginCredentialsModel loginCredentials)
        {
            var token = _tokenService.GetToken(loginCredentials);
            if (!_taskStatus.HasErrors)
            {
                return Ok(token);
            }
            return BadRequest(_taskStatus.ErrorCollection);
        }
    }
}