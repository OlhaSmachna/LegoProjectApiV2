using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LegoProjectApiV2.Services;
using LegoProjectApiV2.Services.Users;
using LegoProjectApiV2.Models.DTOs.User;

namespace LegoProjectApi.Controllers
{
    [Route("lego_project_api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ApiServiceResponseProducer _responseProducer;

        public UsersController(IUserService userService)
        {
            _userService = userService;
            _responseProducer = new ApiServiceResponseProducer();
        }

        // GET: lego_project_api/Users
        [HttpGet]
        public async Task<ApiServiceResponse<List<UserDTO>>> Get()
        {
            return await _userService.GetUsers();
        }

        // GET lego_project_api/Users/5
        [HttpGet("{id}")]
        public async Task<ApiServiceResponse<UserDTO>> Get(int id)
        {
            return await _userService.GetUserById(id);
        }

        // POST lego_project_api/Users
        [HttpPost]
        [AllowAnonymous]
        public async Task<ApiServiceResponse<AddResponceUserDTO>> Post([FromBody] AddRequestUserDTO addRequestUserDTO)
        {
            return await _userService.CreateUser(addRequestUserDTO);
        }

        // POST lego_project_api/Users/Login
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ApiServiceResponse<LoginResponseUserDTO>> Login([FromBody] LoginRequestUserDTO loginRequestUserDTO)
        {
            return await _userService.Login(loginRequestUserDTO);
        }

        // POST lego_project_api/Users/Refresh
        [HttpPost("Refresh")]
        [AllowAnonymous]
        public async Task<ApiServiceResponse<LoginResponseUserDTO>> Refresh([FromBody] RefreshTokenRequestUserDTO refreshTokenRequestUserDTO)
        {
            return await _userService.RefreshToken(refreshTokenRequestUserDTO);
        }

        // PUT lego_project_api/Users/5
        [HttpPut("{id}")]
        public HttpResponseMessage Put([FromBody] EditRequestUserDTO editRequestUserDTO)
        {
            //return await _userService.UpdateUser(editRequestUserDTO);
            return new HttpResponseMessage(HttpStatusCode.NotImplemented);
        }

        // DELETE lego_project_api/Users/5
        [HttpDelete("{id}")]
        public HttpResponseMessage Delete(int id)
        {
            //return await _userService.DeleteUser(id);
            return new HttpResponseMessage(HttpStatusCode.NotImplemented);
        }
    }
}
