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
    }
}
