using Neac.Common.Dtos;
using Neac.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neac.BusinessLogic.Contracts
{
    public interface IDocumentRepository
    {
        Task<Response<GetListResponseModel<List<Document>>>> GetFilter(string filter);
        Task<Response<List<Document>>> GetDropdownAsync();
        Task<Response<Document>> GetByIdAsync(Guid projectId);
        Task<Response<Document>> CreateAsync(Document request);
        Task<Response<Document>> UpdateAsync(Document request);
        Task<Response<bool>> DeleteAsync(Guid projectId);
    }
}
