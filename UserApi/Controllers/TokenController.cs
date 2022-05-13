// using Microsoft.AspNetCore.Authentication;
// using UserApi.Filters;
using UserApi.Filters;
using Microsoft.AspNetCore.Mvc;
using UserApi.DTO;


namespace UserApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class TokenController : ControllerBase
    {
        private readonly IAuthenticationService _authService;

        public TokenController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public IActionResult CreateToken(UserDTO user)
        {
            try
            {
                var token = _authService.GenerateToken(user);
                if (token == null)
                    return Unauthorized();
                return Ok( new { token = token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
