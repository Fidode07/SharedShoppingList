using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedShoppingListApi.Data;
using SharedShoppingListApi.Dtos;
using SharedShoppingListApi.Models;

namespace SharedShoppingListApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly MainDbContext _mainDbContext;

        public AuthController(MainDbContext mainDbContext)
        {
            _mainDbContext = mainDbContext;
        }
        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login(LoginDto loginDto)
        {
            var serviceResponse = new ServiceResponse<string>();
            var user = await _mainDbContext.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == loginDto.Username.ToLower());

            if(user == null)
            {
                serviceResponse.StatusCode = 404;
                serviceResponse.Message = "User not found";
                return await Task.FromResult(StatusCode(serviceResponse.StatusCode, serviceResponse));
            }

            if(!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                serviceResponse.StatusCode = 401;
                serviceResponse.Message = "Password is incorrect";
                return await Task.FromResult(StatusCode(serviceResponse.StatusCode, serviceResponse));
            }

            serviceResponse.Data = user.UniqueId;
            serviceResponse.StatusCode = 200;
            serviceResponse.Success = true;
            serviceResponse.Message = "Successfully logged in";

            return await Task.FromResult(StatusCode(serviceResponse.StatusCode, serviceResponse));
        }

        [HttpPost("register")]
        public async Task<ActionResult<ServiceResponse<string>>> Register(RegisterDto registerDto)
        {
            var serviceResponse = new ServiceResponse<string>();

            if(await _mainDbContext.Users.AnyAsync(u => u.Username.ToLower() == registerDto.Username.ToLower()))
            {
                serviceResponse.StatusCode = 400;
                serviceResponse.Message = "Username already exists";
                return await Task.FromResult(StatusCode(serviceResponse.StatusCode, serviceResponse));
            }

            var newUser = new User
            {
                Username = registerDto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                UniqueId = Guid.NewGuid().ToString("N").Substring(0, 20)
        };

            _mainDbContext.Users.Add(newUser);
            await _mainDbContext.SaveChangesAsync();

            serviceResponse.Data = newUser.UniqueId;
            serviceResponse.StatusCode = 200;
            serviceResponse.Success = true;
            serviceResponse.Message = "Successfully registered";

            return await Task.FromResult(StatusCode(serviceResponse.StatusCode, serviceResponse));
        }
    }
}
