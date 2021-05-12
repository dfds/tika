using System.Collections.Generic;
using System.Threading.Tasks;
using Tika.RestClient.Features.ApiKeys.Models;

namespace Tika.RestClient.Features.ApiKeys
{
    public interface IApiKeysClient
    {
        Task<IEnumerable<ApiKey>> GetAllAsync(string clusterId = null);
        Task<ApiKey> CreateAsync(ApiKeyCreate apiKeyCreate, string clusterId = null);
        Task DeleteAsync(string key, string clusterId = null);
    }
}