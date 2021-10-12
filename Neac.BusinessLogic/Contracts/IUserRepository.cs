using Microsoft.AspNetCore.Http;
using Neac.Common.Dtos;
using Neac.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neac.BusinessLogic.Contracts
{
    public interface IUserRepository
    {
        Task<Response<UserLoginResponseDto>> Login(UserLoginDto request);
        Task<Response<User>> GetUserByUserName(string userName);
        Task<Response<User>> GetUserByUserId(Guid userId);
        Task<Response<GetListResponseModel<List<UserCreateDto>>>> GetListUser(GetListUserRequestDto request);
        Task<Response<UserCreateDto>> Create(UserCreateDto request);
        Task<Response<UserCreateDto>> Update(UserCreateDto request);
        Task<Response<bool>> Delete(Guid userId);
        Task<Response<bool>> DeleteMany(List<Guid> userId);
        Task<UserCreateDto> GetIdentityUser();
        Task<Response<string>> UploadAvatar(IFormFile avatar);
    }
}
