using Azure;
using Dapper;
using FoodRecipes.Context;
using FoodRecipes.Contracts;
using FoodRecipes.Dto;
using FoodRecipes.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FoodRecipes.Repository
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
            var query = "SELECT Id, Username, Email FROM Users";

            using (var connection = _context.CreateConnection())
            {
                var users = await connection.QueryAsync<User>(query);
                return users.ToList();
            }
        }

        public async Task<User> GetUserById(int id)
        {
            var query = "SELECT * FROM Users WHERE Id = @Id";

            using (var connection = _context.CreateConnection())
            {
                var user = await connection.QuerySingleOrDefaultAsync<User>(query, new { id });

                return user;
            }
        }

        public async Task<User> GetUser(string username)
        {
            var query = "SELECT * FROM Users WHERE Username = @Username";

            using (var connection = _context.CreateConnection())
            {
                var user = await connection.QuerySingleOrDefaultAsync<User>(query, new { username });

                return user;
            }
        }

        public async Task<User> Register(UserRegisterDto user)
        {
            var query = "INSERT INTO Users (Username, Email, Password) VALUES (@Username, @Email, @Password)" +
                "SELECT CAST(SCOPE_IDENTITY() as int)";

            var parameters = new DynamicParameters();
            parameters.Add("Username", user.Username, DbType.String);
            parameters.Add("Email", user.Email, DbType.String);
            parameters.Add("Password", user.Password, DbType.String);

            using (var connection = _context.CreateConnection())
            {
                var id = await connection.QuerySingleAsync<int>(query, parameters);

                var createdUser = new User
                {
                    Id = id,
                    Username = user.Username,
                    Email = user.Email,
                    Password = user.Password
                };

                return createdUser;
            }
        }

        public async Task<User> UpdateUser(int id, UserUpdateDto user)
        {
            var query = "UPDATE Users SET Username = @Username, Email = @Email, Password = @Password WHERE Id = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32);
            parameters.Add("Username", user.Username, DbType.String);
            parameters.Add("Email", user.Email, DbType.String);
            parameters.Add("Password", user.Password, DbType.String);

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);

                var updatedUser = new User
                {
                    Id = id,
                    Username = user.Username,
                    Email = user.Email,
                    Password = user.Password
                };

                return updatedUser;
            }
        }

        public async Task<string> DeleteUser(int id)
        {
            var query = "DELETE FROM Users WHERE Id = @Id";

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, new { id });
                return "User successfully removed.";
            }
        }

        public async Task<int> Login(string username, string password)
        {
            var user = await GetUser(username);

            if (user is null)
            {
                // return "Username not found.";
                return -1;
            }
            else if (password != user.Password)
            {
                //return "Wrong password.";
                return -1;
            }

            DataHelper.LoggedUserId = user.Id;
            return user.Id;
        }

        public async Task<bool> UserExists(string username)
        {
            var users = await GetUsers();
            if (users.Any(u => u.Username.ToLower() == username.ToLower()))
            {
                return true;
            }
            return false;
        }      
    }
}
