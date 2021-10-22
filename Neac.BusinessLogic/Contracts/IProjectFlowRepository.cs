using Microsoft.AspNetCore.Http;
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
        Task<Response<List<ProjectFlow>>> GetFilterAsync(string filter);
        Task<Response<ProjectFlow>> CreateAsync();
        Task<Response<ProjectFlow>> UpdateAsync(ProjectFlow request);
        Task<Response<List<ProjectFlowCurrentDto>>> CurrentState(Guid projectId);
        Task<Response<Guid>> CurrentPackageAsync(Guid projectId);
        Task<string> UploadFile(IFormFile file);
        Task<string> DownloadAsync(Guid projectFlowId);
    }
}
