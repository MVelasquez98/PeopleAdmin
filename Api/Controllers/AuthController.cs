using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Core.PersonRepository.Interfaces;
using Data.Model;
namespace Api.Controllers
{
    [Route("api/authSecurity")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult RequestToken([FromBody] LoginModel login)
        {
            var token = _authRepository.RequestToken(login);

            if (!string.IsNullOrEmpty(token))
            {
                return Ok(new { token });
            }

            return BadRequest("Could not verify username and password");
        }
    }

}