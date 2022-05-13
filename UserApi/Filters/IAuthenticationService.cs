using UserApi.DTO;
using UserApi.Models;

namespace UserApi.Filters
{
    public interface IAuthenticationService
    {
        public string GenerateToken(UserDTO user);
    }
}
