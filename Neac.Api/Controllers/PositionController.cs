using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neac.Api.Attributes;
using Neac.BusinessLogic.Contracts;
using Neac.Common.Dtos;
using Neac.Common.Dtos.PositionDtos;
using Neac.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neac.Api.Controllers
{
    [UserAuthorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PositionController : ControllerBase
    {
        private readonly IPositionRepository _positionRepository;
        public PositionController(IPositionRepository positionRepository)
        {
            _positionRepository = positionRepository;
        }

        [RoleDescription("Xem danh sách nhóm tài khoản")]
        [Route("")]
        [HttpGet]
        public async Task<Response<GetListResponseModel<List<UserPosition>>>> GetUserPositionsAsync(string filter)
        {
            return await _positionRepository.GetUserPositionsAsync(filter);
        }

        [RoleDescription("Lấy dropdown nhóm tài khoản")]
        [Route("dropdown")]
        [HttpGet]
        public async Task<Response<List<PositonGetDropdownViewDto>>> GetUserPositionsDropdownAsync()
        {
            return await _positionRepository.GetUserPositionsDropdownAsync();
        }
    }
}
