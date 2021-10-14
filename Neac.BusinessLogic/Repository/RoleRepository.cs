using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Neac.BusinessLogic.Contracts;
using Neac.BusinessLogic.UnitOfWork;
using Neac.Common.Dtos;
using Neac.Common.Dtos.RoleDtos;
using Neac.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neac.BusinessLogic.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ILogRepository _logRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IMemoryCache _memoryCache;
        public RoleRepository(IUnitOfWork unitOfWork, IUserRepository userRepository, IMemoryCache memoryCache, ILogRepository logRepository)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _memoryCache = memoryCache;
            _logRepository = logRepository;
        }

        public async Task<Response<GetRolesAndGroupDto>> DecentralizatedRole(Guid userId)
        {
            try
            {
                var roleSelected = await _unitOfWork.GetRepository<UserRole>().GetAll().Where(n => n.UserId == userId).Select(n => n.RoleId.Value).ToListAsync();

                var listRole = await _unitOfWork.GetRepository<GroupRole>().GetAll().Include(n => n.Roles).Select(n => new DecentralizatedDto
                { 
                    Data = n.GroupRoleId,
                    Label = n.GroupRoleName,
                    Children = n.Roles.Select(g => new DecentralizatedDto { Data = g.RoleId, Label = g.RoleName })
                }).ToListAsync();

                return Response<GetRolesAndGroupDto>.CreateSuccessResponse(new GetRolesAndGroupDto {ListRole = listRole, SelectedIds = roleSelected });
            }
            catch(Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<GetRolesAndGroupDto>.CreateErrorResponse(ex);
            }
        }

        public async Task<Response<GetRolesByUserDtos>> GetUserRole(Guid userId)
        {
            try
            {
                var userRoleMem = _memoryCache.Get($"user/{userId}");
                if (userRoleMem != null)
                {
                    return Response<GetRolesByUserDtos>.CreateSuccessResponse((GetRolesByUserDtos)userRoleMem);
                }
                var roles = await (from ur in _unitOfWork.GetRepository<UserRole>().GetAll()
                                   join r in _unitOfWork.GetRepository<Role>().GetAll() on ur.RoleId equals r.RoleId
                                   where ur.UserId == userId
                                   select r).ToListAsync();

                var userRoles = new GetRolesByUserDtos { UserId = userId, Roles = roles };
                _memoryCache.Set($"user/{userId}", userRoles);

                return Response<GetRolesByUserDtos>.CreateSuccessResponse(userRoles);
            }
            catch(Exception ex)
            {
                return Response<GetRolesByUserDtos>.CreateErrorResponse(ex);
            }
        }

        public async Task<Response<bool>> UpdateListRole(List<Role> roles, IEnumerable<GroupRole> groupRoles)
        {
            try
            {
                var listRole = _unitOfWork.GetRepository<Role>().GetAll();
                var listGroupRole = _unitOfWork.GetRepository<GroupRole>().GetAll();
                var currentUser = await _userRepository.GetIdentityUser();

                // cập nhật lại group user Role
                await AddOrCreateGroupRoleAsync(groupRoles, listGroupRole);

                // cập nhật lại vào danh sách role
                foreach (var role in roles)
                {
                    
                    var roleCode = await listRole.FirstOrDefaultAsync(n => n.RoleCode == role.RoleCode);
                    if (roleCode != null)
                    {
                        roleCode.RoleName = role.RoleName;
                        roleCode.ModifiedBy = currentUser.UserId;
                        roleCode.ModifiedDate = DateTime.Now;
                        roleCode.GroupRoleId = await GetGroupRoleId(role.RoleCode, listGroupRole);
                        await _unitOfWork.GetRepository<Role>().Update(roleCode);
                    }
                    else
                    {
                        
                        role.RoleId = Guid.NewGuid();
                        role.CreatedBy = currentUser.UserId;
                        role.CreatedDate = DateTime.Now;
                        role.GroupRoleId = await GetGroupRoleId(role.RoleCode, listGroupRole);
                        await _unitOfWork.GetRepository<Role>().Add(role);
                    }
                }

                // lấy ra những tài khoản quản trị
                var adminUsers = await (from u in _unitOfWork.GetRepository<User>().GetAll()
                                        join up in _unitOfWork.GetRepository<UserPosition>().GetAll() on u.UserPositionId equals up.UserPositionId
                                        where up.IsAdministrator.Value
                                        select u).ToListAsync();
                // xóa role cũ của admin
                await _unitOfWork.GetRepository<UserRole>().DeleteByExpression(n => adminUsers.Select(g => g.UserId).Any(g => g == n.UserId));
                await _unitOfWork.SaveAsync();

                // cập nhật role mới
                foreach (var item in adminUsers)
                {
                    var adminRoles = _unitOfWork.GetRepository<Role>().GetAll().Select(n => new UserRole { RoleId = n.RoleId, UserId = item.UserId });
                    await _unitOfWork.GetRepository<UserRole>().AddRangeAsync(adminRoles);
                }

                await _unitOfWork.SaveAsync();

                // update role mới cho quản trị

                return Response<bool>.CreateSuccessResponse(true); ;
            }
            catch(Exception ex)
            {
                return Response<bool>.CreateErrorResponse(ex);
            }
        }

        public async Task<Response<Guid>> UpdateUserRole(UpdateRoleUserDto request)
        {
            try
            {
                var user = await _unitOfWork.GetRepository<User>().GetByExpression(n => n.UserId == request.UserId).Include(n => n.UserRoles).FirstOrDefaultAsync();
                if(user?.UserRoles?.Count > 0)
                {
                    await _unitOfWork.GetRepository<UserRole>().DeleteByExpression(n => user.UserRoles.Select(g => g.UserId).Any(g => g == n.UserId));
                    await _unitOfWork.SaveAsync();
                }
                foreach (var item in request.RoleIds)
                {
                    await _unitOfWork.GetRepository<UserRole>().Add(new UserRole() { RoleId = item, UserId = request.UserId, UserRoleId = Guid.NewGuid() });
                }
                await _unitOfWork.SaveAsync();

                var roles = await (from ur in _unitOfWork.GetRepository<UserRole>().GetAll()
                                   join r in _unitOfWork.GetRepository<Role>().GetAll() on ur.RoleId equals r.RoleId
                                   where ur.UserId == request.UserId
                                   select r).ToListAsync();
                var userRoles = new GetRolesByUserDtos { UserId = request.UserId, Roles = roles };

                _memoryCache.Remove($"user/{request.UserId}");
                _memoryCache.Set($"user/{request.UserId}", userRoles);
                return Response<Guid>.CreateSuccessResponse(request.UserId);
            }
            catch(Exception ex)
            {
                return Response<Guid>.CreateErrorResponse(ex);
            }
        }

        private async Task AddOrCreateGroupRoleAsync(IEnumerable<GroupRole> groupRoles, IQueryable<GroupRole> listGroupRole)
        {
            foreach (var item in groupRoles)
            {
                var groupRoleCode = await listGroupRole.FirstOrDefaultAsync(n => n.GroupRoleCode == item.GroupRoleCode);
                if (groupRoleCode != null)
                {
                    groupRoleCode.GroupRoleName = item.GroupRoleName;
                    await _unitOfWork.GetRepository<GroupRole>().Update(groupRoleCode);
                }
                else
                {

                    item.GroupRoleId = Guid.NewGuid();
                    await _unitOfWork.GetRepository<GroupRole>().Add(item);
                }
            }
            await _unitOfWork.SaveAsync();
        }
        private async Task<Guid> GetGroupRoleId(string roleCode, IQueryable<GroupRole> listGroupRole)
        {
            string roleGroupName = roleCode.Split("-")[0];
            var roleGroup = await listGroupRole.FirstOrDefaultAsync(n => n.GroupRoleCode == roleGroupName);
            return roleGroup.GroupRoleId;
        }
    }
}
