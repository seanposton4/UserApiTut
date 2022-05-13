using UserApi.Contracts;
using UserApi.Context;
using UserApi.Models;
using UserApi.DTO;
using Dapper;
using System.Data;

namespace UserApi.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DapperContext _context;
        public UserRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var query = "SELECT * FROM Users ORDER BY userId ASC";

            using (var connection = _context.CreateConnection())
            {
                var users = await connection.QueryAsync<User>(query);
                return users.ToList();
            }
        }

        public async Task<User> GetUser(int id)
        {
            var query = "SELECT * FROM Users WHERE userId = @id";

            using (var connection = _context.CreateConnection())
            {
                var user = await connection.QuerySingleOrDefaultAsync<User>(query, new { id });
                return user;
            }
        }

        public async Task<User> CreateUser(UserDTO user)
        {
            var query = "INSERT INTO Users (name, password, city, locations) VALUES (@name, @password, @city, @locations)" +
                "SELECT CAST(SCOPE_IDENTITY() AS int)";

            var parameters = new DynamicParameters();
            parameters.Add("name", user.name, DbType.String);
            parameters.Add("city", user.city, DbType.String);
            parameters.Add("locations", user.locations, DbType.String);
            parameters.Add("password", user.password, DbType.String);

            using (var connection = _context.CreateConnection())
            {
                var id = await connection.QuerySingleAsync<int>(query, parameters);

                var createdUser = new User
                {
                    userId = id,
                    name = user.name,
                    password = user.password,
                    city = user.city,
                    locations = user.locations
                };

                return createdUser;
            }
        }

        public async Task UpdateUser(int id, UserDTO user)
        {
            var query = "UPDATE Users SET name = @name, password = @password, city = @city, locations = @locations WHERE userId = @id";

            var parameters = new DynamicParameters();
            parameters.Add("id", id, DbType.Int32);
            parameters.Add("name", user.name, DbType.String);
            parameters.Add("city", user.city, DbType.String);
            parameters.Add("locations", user.locations, DbType.String);
            parameters.Add("password", user.password, DbType.String);


            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
                // await connection.QuerySingleAsync(query, parameters);
            }
        }

        public async Task DeleteUser(int id)
        {
            var query = "DELETE FROM Users WHERE userId = @id";

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, new { id });
            }
        }

        public async Task<User> Login(string username, string password)
        {
            var query = "SELECT * FROM Users WHERE name = @name AND password = @password";

            var parameters = new DynamicParameters();
            parameters.Add("name", username, DbType.String);
            parameters.Add("password", password, DbType.String);

            using (var connection = _context.CreateConnection())
            {
                var returnedUser = await connection.QuerySingleAsync(query, parameters);
                
                var user = new User
                {
                    userId = returnedUser.userId,
                    name = returnedUser.name,
                    password = returnedUser.password,
                    city = returnedUser.city,
                    locations = returnedUser.locations,
                };

                return user;
            }
        }
    }
}