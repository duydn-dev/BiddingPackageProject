using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neac.Api.Attributes;
using Neac.BusinessLogic.Contracts;
using Neac.Common.Dtos;
using Neac.DataAccess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Neac.Api.Controllers
{
    [UserAuthorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [Route("login")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<Response<UserLoginResponseDto>> Login([FromBody] UserLoginDto request)
        {
            return await _userRepository.Login(request);
        }

        [Route("register")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<Response<UserCreateDto>> Register([FromBody] UserCreateDto request)
        {
            request.UserId = Guid.NewGuid();
            request.CreatedBy = request.UserId;
            return await _userRepository.Create(request);
        }

        [RoleDescription("Xem danh sách tài khoản")]
        [Route("")]
        [HttpGet]
        public async Task<Response<GetListResponseModel<List<UserCreateDto>>>> GetFilter(string request)
        {
            var req = JsonConvert.DeserializeObject<GetListUserRequestDto>(request);
            return await _userRepository.GetListUser(req);
        }

        [RoleDescription("Xem tài khoản bằng Id")]
        [Route("{userId}")]
        [HttpGet]
        public async Task<Response<User>> GetById(Guid userId)
        {
            return await _userRepository.GetUserByUserId(userId);
        }

        [RoleDescription("Thêm mới tài khoản")]
        [Route("create")]
        [HttpPost]
        public async Task<Response<UserCreateDto>> Create([FromBody] UserCreateDto request)
        {
            return await _userRepository.Create(request);
        }

        [RoleDescription("Cập nhật tài khoản")]
        [Route("update/{userId}")]
        [HttpPut]
        public async Task<Response<UserCreateDto>> Update(Guid userId, [FromBody] UserCreateDto request)
        {
            request.UserId = userId;
            return await _userRepository.Update(request);
        }

        [RoleDescription("Xóa tài khoản")]
        [Route("delete/{userId}")]
        [HttpDelete]
        public async Task<Response<bool>> Delete(Guid userId)
        {
            return await _userRepository.Delete(userId);
        }

        [RoleDescription("Xóa nhiều tài khoản")]
        [Route("delete")]
        [HttpDelete]
        public async Task<Response<bool>> Delete([FromQuery]List<Guid> userIds)
        {
            return await _userRepository.DeleteMany(userIds);
        }
        
        [RoleDescription("Upload avatar người dùng")]
        [Route("upload-avatar")]
        [HttpPost]
        public async Task<Response<string>> UploadAvatar()
        {
            return await _userRepository.UploadAvatar(Request.Form.Files[0]);
        }
    }
}
