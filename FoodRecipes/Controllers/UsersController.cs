using Azure;
using FoodRecipes.Contracts;
using FoodRecipes.Dto;
using FoodRecipes.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodRecipes.Controllers
{
    [Route("api/users")]
    [ApiController]
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
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}", Name = "UserById")]
        public async Task<IActionResult> GetUser(int id)
        {
            try
            {
                var response = await _userRepo.GetUserById(id);
                if (response == null)
                    return NotFound();

                return Ok(response);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegisterDto user)
        {
            try
            {
                if (user.Username == string.Empty || user.Username is null)
                {
                    return StatusCode(409, "Username field is required.");
                }

                if (user.Password == string.Empty || user.Password is null)
                {
                    return StatusCode(409, "Password field is required.");
                }

                if (await _userRepo.UserExists(user.Username))
                {
                    return StatusCode(409, "User already exists.");
                }

                var response = await _userRepo.Register(
                new UserRegisterDto { Username = user.Username, Email = user.Email, Password = user.Password});
                return Ok(response);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDto user)
        {
            try
            {
                var response = await _userRepo.Login(user.Username, user.Password);
                return Ok(response);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserUpdateDto user)
        {
            try
            {
                var response = await _userRepo.GetUserById(id);
                if (response == null)
                    return NotFound();

                await _userRepo.UpdateUser(id, user);
                return NoContent();
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var dbCompany = await _userRepo.GetUserById(id);
                if (dbCompany == null)
                    return NotFound();

                await _userRepo.DeleteUser(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
    }
}
