using Neac.Common.Dtos;
using Neac.Common.Dtos.BiddingPackage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neac.BusinessLogic.Contracts
{
    public interface IBiddingPackageRepository
    {
        Task<Response<GetListResponseModel<List<BiddingPackageDto>>>> GetFilter(string filter);
        Task<Response<List<BiddingPackageByIdDto>>> GetDropdownAsync();
        Task<Response<List<BiddingPackageDto>>> GetDropdownByProjectAsync(Guid projectId);
        Task<Response<BiddingPackageDto>> GetByIdAsync(Guid projectId);
        Task<Response<BiddingPackageByIdDto>> GetByBiddingPackageIdAsync(Guid biddingPackageId);
        Task<Response<BiddingPackageByIdDto>> CreateAsync(BiddingPackageByIdDto request);
        Task<Response<BiddingPackageByIdDto>> UpdateAsync(BiddingPackageByIdDto request);
        Task<Response<bool>> DeleteAsync(Guid projectId);
    }
}
