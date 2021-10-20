using Neac.Common.Dtos;
using Neac.Common.Dtos.ProjectDtos;
using Neac.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neac.BusinessLogic.Contracts
{
    public interface IProjectFlowRepository
    {
        Task<Response<GetListResponseModel<List<ProjectFlow>>>> GetFilterAsync(string filter);
        Task<Response<ProjectFlow>> CreateAsync(ProjectFlow request);
        Task<Response<ProjectFlow>> UpdateAsync(ProjectFlow request);
        Task<Response<List<ProjectFlowCurrentDto>>> CurrentState(Guid projectId);
    }
}
