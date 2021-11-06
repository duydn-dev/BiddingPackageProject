using Neac.Common.Dtos;
using Neac.Common.Dtos.ProjectDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neac.BusinessLogic.Contracts
{
    public interface IProjectRepository
    {
        Task<Response<GetListResponseModel<List<ProjectGetListDto>>>> GetFilter(string filter);
        Task<Response<ProjectGetListDto>> GetByIdAsync(Guid projectId);
        Task<Response<ProjectGetListDto>> CreateAsync(ProjectGetListDto request);
        Task<Response<ProjectGetListDto>> UpdateAsync(ProjectGetListDto request);
        Task<Response<bool>> DeleteAsync(Guid projectId); 
        Task<Response<IEnumerable<ExportDataDto>>> GetExportDataAsync(Guid projectId);
        Task<Response<ProjectGetStatisticalDto>> GetProjectStatisticalAsync();
    }
}
