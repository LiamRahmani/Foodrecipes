using FoodRecipes.Dto;
using FoodRecipes.Entities;

namespace FoodRecipes.Contracts
{
    public interface IUserRepository
    {
        public Task<IEnumerable<User>> GetUsers();
        public Task<User> GetUserById(int id);
        public Task<User> UpdateUser(int id, UserUpdateDto user);
        public Task<string> DeleteUser(int id);
        public Task<User> Register(UserRegisterDto user);
        public Task<int> Login(string username, string password);
        public Task<bool> UserExists(string username);
    }
}
