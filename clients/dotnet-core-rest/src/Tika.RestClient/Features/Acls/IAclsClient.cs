using System.Collections.Generic;
using System.Threading.Tasks;
using Tika.RestClient.Features.Acls.Models;

namespace Tika.RestClient.Features.Acls
{
    public interface IAclsClient
    {
        Task<IEnumerable<Acl>> GetAllAsync(string clusterId = null);
        Task CreateAsync(AclCreateDelete aclCreateDelete, string clusterId = null);
        Task DeleteAsync(AclCreateDelete aclDelete, string clusterId = null);
    }
}