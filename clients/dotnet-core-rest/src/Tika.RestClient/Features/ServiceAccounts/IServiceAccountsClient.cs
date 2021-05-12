using System.Collections.Generic;
using System.Threading.Tasks;
using Tika.RestClient.Features.ServiceAccounts.Models;

namespace Tika.RestClient.Features.ServiceAccounts
{
    public interface IServiceAccountsClient
    {
        Task<IEnumerable<ServiceAccount>> GetAllAsync(string clusterId = null);
        Task<ServiceAccount> CreateAsync(ServiceAccountCreateCommand serviceAccountCreateCommand, string clusterId = null);
        Task DeleteAsync(string id, string clusterId = null);
    }
}