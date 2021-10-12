using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neac.BusinessLogic.Contracts
{
    public interface ILogRepository
    {
        Task InfoAsync(string message);
        Task ErrorAsync(Exception ex);
        Task ErrorAsync(string message);
    }
}
