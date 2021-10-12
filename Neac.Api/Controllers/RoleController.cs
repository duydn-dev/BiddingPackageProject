using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neac.Api.Attributes;
using Neac.BusinessLogic.Contracts;
using Neac.Common.Dtos;
using Neac.Common.Dtos.RoleDtos;
using Neac.DataAccess;
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
    [RoleGroupDescription("Quản lý người dùng")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;
        public RoleController(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        [RoleDescription("Cập nhật danh sách quyền")]
        [Route("update-role")]
        [HttpPost]
        public async Task<Response<bool>> UpdateRole()
        {
            
            Assembly asm = Assembly.GetExecutingAssembly();
            var listController = asm.GetTypes()
                .Where(type => typeof(ControllerBase).IsAssignableFrom(type))
                .Select(n => new GroupRole { GroupRoleId = Guid.Empty, GroupRoleCode = n.Name.Replace("Controller", ""), GroupRoleName = ((RoleGroupDescriptionAttribute)n.GetCustomAttribute(typeof(RoleGroupDescriptionAttribute)))?.Description });

            var controlleractionlist = asm.GetTypes()
                    .Where(type => typeof(ControllerBase).IsAssignableFrom(type))
                    .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                    .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
                    .Where(m => !m.CustomAttributes.Any(n => n.AttributeType == typeof(AllowAnonymousAttribute)))
                    .Select(x => new Role
                    {
                        RoleCode = x.DeclaringType.Name.Replace("Controller", "") + "-" + x.Name,
                        RoleId = Guid.Empty,
                        RoleName = ((RoleDescriptionAttribute)x.GetCustomAttribute(typeof(RoleDescriptionAttribute)))?.Description
                    })
                    .OrderBy(x => x.RoleCode).ToList();
            return await _roleRepository.UpdateListRole(controlleractionlist, listController);
        }

        [RoleDescription("Phân quyền tài khoản")]
        [Route("update-user-role")]
        [HttpPost]
        public async Task<Response<Guid>> UpdateUserRole([FromBody]UpdateRoleUserDto request)
        {
            return await _roleRepository.UpdateUserRole(request);
        }

        [RoleDescription("Lấy danh sách quyền theo tài khoản")]
        [Route("get-user-role/{userId}")]
        [HttpGet]
        public async Task<Response<GetRolesByUserDtos>> GetUserRole(Guid userId)
        {
            return await _roleRepository.GetUserRole(userId);
        }
    }
}
