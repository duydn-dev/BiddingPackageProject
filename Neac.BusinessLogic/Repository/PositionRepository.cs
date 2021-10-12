using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Neac.BusinessLogic.Contracts;
using Neac.BusinessLogic.UnitOfWork;
using Neac.Common;
using Neac.Common.Dtos;
using Neac.Common.Dtos.PositionDtos;
using Neac.DataAccess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neac.BusinessLogic.Repository
{
    public class PositionRepository : IPositionRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogRepository _logRepository;
        private readonly IUnitOfWork _unitOfWork;
        public PositionRepository(ILogRepository logRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logRepository = logRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Response<GetListResponseModel<List<UserPosition>>>> GetUserPositionsAsync(string filter)
        {
            try
            {
                var request = JsonConvert.DeserializeObject<PositionGetFilterDto>(filter);
                var query = _unitOfWork.GetRepository<UserPosition>()
                                .GetAll()
                                .WhereIf(!string.IsNullOrEmpty(request.TextSearch), n => n.UserPositionName.Contains(request.TextSearch));

                GetListResponseModel<List<UserPosition>> responseData = new GetListResponseModel<List<UserPosition>>(query.Count(), request.PageSize);
                var result = await query
                    .OrderByDescending(n => n.CreatedDate)
                    .Skip(request.PageSize * (request.PageIndex - 1)).Take(request.PageSize)
                    .ToListAsync();
                return Response<GetListResponseModel<List<UserPosition>>>.CreateSuccessResponse(responseData);
            }
            catch(Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<GetListResponseModel<List<UserPosition>>>.CreateErrorResponse(ex);
            }
        }

        public async Task<Response<List<PositonGetDropdownViewDto>>> GetUserPositionsDropdownAsync()
        {
            try
            {
                var data = await _unitOfWork.GetRepository<UserPosition>().GetAll().ToListAsync();
                return Response<List<PositonGetDropdownViewDto>>.CreateSuccessResponse(_mapper.Map<List<UserPosition>, List<PositonGetDropdownViewDto>>(data));
            }
            catch(Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<List<PositonGetDropdownViewDto>>.CreateErrorResponse(ex);
            }
        }
    }
}
