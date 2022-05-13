using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserApi.Contracts;
using UserApi.DTO;
using UserApi.Filters;

namespace UserApi.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize]

    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepo;

        public UsersController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _userRepo.GetUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}", Name = "UserById")]
        public async Task<IActionResult> GetUser(int id)
        {
            try
            {
                var user = await _userRepo.GetUser(id);
                if (user == null) { return NotFound(); }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserDTO user)
        {
            try
            {
                var createdUser = await _userRepo.CreateUser(user);
                return CreatedAtRoute("UserById", new { id = createdUser.userId }, createdUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserDTO user)
        {
            try
            {
                var userInfo = await _userRepo.Login(user.name, user.password);
                return Ok(userInfo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserDTO user)
        {
            try
            {
                var dbUser = await _userRepo.GetUser(id);
                if (dbUser == null)
                    return NotFound();

                await _userRepo.UpdateUser(id, user);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var dbUser = await _userRepo.GetUser(id);
                if (dbUser == null)
                    return NotFound();

                await _userRepo.DeleteUser(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
