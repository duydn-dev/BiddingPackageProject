using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Neac.BusinessLogic.Contracts;
using Neac.BusinessLogic.UnitOfWork;
using Neac.Common;
using Neac.Common.Dtos;
using Neac.Common.Dtos.BiddingPackage;
using Neac.DataAccess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neac.BusinessLogic.Repository
{
    public class BiddingPackageRepository : IBiddingPackageRepository
    {
        private readonly ILogRepository _logRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public BiddingPackageRepository(ILogRepository logRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logRepository = logRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Response<GetListResponseModel<List<BiddingPackageDto>>>> GetFilter(string filter)
        {
            try
            {
                var request = JsonConvert.DeserializeObject<GetListUserRequestDto>(filter);
                var query = _unitOfWork.GetRepository<BiddingPackage>()
                                    .GetAll()
                                    .WhereIf(!string.IsNullOrEmpty(request.TextSearch), n => n.BiddingPackageName.Contains(request.TextSearch));

                GetListResponseModel<List<BiddingPackageDto>> responseData = new GetListResponseModel<List<BiddingPackageDto>>(query.Count(), request.PageSize);
                var result = await query
                    .OrderByDescending(n => n.BiddingPackageName)
                    .Skip(request.PageSize * (request.PageIndex - 1)).Take(request.PageSize)
                    .ToListAsync();

                responseData.Data = _mapper.Map<List<BiddingPackage>, List<BiddingPackageDto>>(result);
                return Response<GetListResponseModel<List<BiddingPackageDto>>>.CreateSuccessResponse(responseData);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<GetListResponseModel<List<BiddingPackageDto>>>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<BiddingPackageDto>> GetByIdAsync(Guid BiddingPackageId)
        {
            try
            {
                var query = await _unitOfWork.GetRepository<BiddingPackage>().GetByExpression(n => n.BiddingPackageId == BiddingPackageId).FirstOrDefaultAsync();
                return Response<BiddingPackageDto>.CreateSuccessResponse(_mapper.Map<BiddingPackage, BiddingPackageDto>(query));
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<BiddingPackageDto>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<BiddingPackageDto>> CreateAsync(BiddingPackageDto request)
        {
            try
            {
                request.BiddingPackageId = Guid.NewGuid();
                var mappped = _mapper.Map<BiddingPackageDto, BiddingPackage>(request);
                await _unitOfWork.GetRepository<BiddingPackage>().Add(mappped);
                await _unitOfWork.SaveAsync();
                return Response<BiddingPackageDto>.CreateSuccessResponse(request);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<BiddingPackageDto>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<BiddingPackageDto>> UpdateAsync(BiddingPackageDto request)
        {
            try
            {
                var BiddingPackage = await _unitOfWork.GetRepository<BiddingPackage>().GetByExpression(n => n.BiddingPackageId == request.BiddingPackageId).FirstOrDefaultAsync();
                var mappped = _mapper.Map<BiddingPackageDto, BiddingPackage>(request, BiddingPackage);
                await _unitOfWork.GetRepository<BiddingPackage>().Update(mappped);
                await _unitOfWork.SaveAsync();
                return Response<BiddingPackageDto>.CreateSuccessResponse(request);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<BiddingPackageDto>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<bool>> DeleteAsync(Guid BiddingPackageId)
        {
            try
            {
                await _unitOfWork.GetRepository<BiddingPackage>().DeleteByExpression(n => n.BiddingPackageId == BiddingPackageId);
                await _unitOfWork.SaveAsync();
                return Response<bool>.CreateSuccessResponse(true);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<bool>.CreateErrorResponse(ex);
            }
        }
    }
}
