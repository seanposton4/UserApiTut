using UserApi.DTO;
using UserApi.Models;
namespace UserApi.Contracts
{
    public interface IUserRepository
    {
        public Task<IEnumerable<User>> GetUsers();
        public Task<User> GetUser(int id);
        public Task<User> CreateUser(UserDTO user);
        public Task UpdateUser(int id, UserDTO user);
        public Task DeleteUser(int id);
        public Task<User> Login(string username, string password);
    }
}
