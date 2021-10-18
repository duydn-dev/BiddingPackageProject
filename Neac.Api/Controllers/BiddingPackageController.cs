using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neac.Api.Attributes;
using Neac.BusinessLogic.Contracts;
using Neac.Common.Dtos;
using Neac.Common.Dtos.BiddingPackage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neac.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [RoleGroupDescription("Quản lý gói thầu")]
    public class BiddingPackageController : ControllerBase
    {
        private readonly IBiddingPackageRepository _biddingPackageRepository;
        public BiddingPackageController(IBiddingPackageRepository biddingPackageRepository)
        {
            _biddingPackageRepository = biddingPackageRepository;
        }

        [Route("")]
        [HttpGet]
        [RoleDescription("Xem danh sách gói")]
        public async Task<Response<GetListResponseModel<List<BiddingPackageDto>>>> GetFilterAsync([FromQuery] string filter)
        {
            return await _biddingPackageRepository.GetFilter(filter);
        }

        [Route("{packageId}")]
        [HttpGet]
        [RoleDescription("Xem chi tiết gói thầu")]
        public async Task<Response<BiddingPackageDto>> GetByIdAsync(Guid packageId)
        {
            return await _biddingPackageRepository.GetByIdAsync(packageId);
        }

        [Route("with-document/{packageId}")]
        [HttpGet]
        [RoleDescription("Xem chi tiết gói thầu - bao gồm văn bản")]
        public async Task<Response<BiddingPackageByIdDto>> GetByBiddingPackageIdAsync(Guid packageId)
        {
            return await _biddingPackageRepository.GetByBiddingPackageIdAsync(packageId);
        }

        [Route("create")]
        [HttpPost]
        [RoleDescription("Thêm mới gói thầu")]
        public async Task<Response<BiddingPackageDto>> CreateAsync(BiddingPackageDto request)
        {
            return await _biddingPackageRepository.CreateAsync(request);
        }
        [Route("update/{packageId}")]
        [HttpPut]
        [RoleDescription("Cập nhật gói thầu")]
        public async Task<Response<BiddingPackageDto>> UpdateAsync([FromRoute] Guid packageId, BiddingPackageDto request)
        {
            request.BiddingPackageId = packageId;
            return await _biddingPackageRepository.UpdateAsync(request);
        }
        [Route("delete/{packageId}")]
        [HttpDelete]
        [RoleDescription("Xóa gói thầu")]
        public async Task<Response<bool>> DeleteAsync(Guid packageId)
        {
            return await _biddingPackageRepository.DeleteAsync(packageId);
        }
    }
}
