using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tika.RestClient.Features.Acls.Models;
using Tika.RestClient.Features.ServiceAccounts.Models;
using Tika.RestClient.IntegrationTests.Features.Factories;
using Xunit;

namespace Tika.RestClient.IntegrationTests.Features.Acls
{
    public class GetAllAclsScenario
    {
        private IRestClient _client;
        private ServiceAccountCreateCommand _serviceAccountCreateCommand;
        private ServiceAccount _serviceAccount;
        private AclCreateDelete _aclCreateDelete;
        private IEnumerable<Acl> _returnedAcls;

        [Fact]
        public async Task GetAllWithSingleAclScenario()
        {
                  Given_an_acl_client();
            await And_a_single_acl();
            await When_GetAll_is_called();
                  Then_the_acl_is_returned();
        }
        
        private void Given_an_acl_client()
        {
            _client = LocalhostRestClient.Create();
        }
        
        private async Task And_a_single_acl()
        {
            using var client = LocalhostRestClient.Create();
            _serviceAccountCreateCommand = ServiceAccountCreateCommandFactory.CreateForIntegrationTest();
            
            _serviceAccount = await client.ServiceAccounts.CreateAsync(_serviceAccountCreateCommand);
            
            _aclCreateDelete = new AclCreateDelete
            {
                ServiceAccountId = Convert.ToInt64(_serviceAccount.Id),
                Allow = true,
                Operation = "WRITE",
                TopicPrefix = "itsAThing"
            };

            await client.Acls.CreateAsync(_aclCreateDelete);
        }
        
        private async Task When_GetAll_is_called()
        {
            _returnedAcls = await _client.Acls.GetAllAsync();
        }
        
        private void Then_the_acl_is_returned()
        {
            var serviceAccount = _returnedAcls
                .Where(acl => acl.Operation == _aclCreateDelete.Operation)
                .Where(acl => acl.Name == _aclCreateDelete.TopicPrefix)
                .Where(acl => acl.ConsumerGroupPrefix == _aclCreateDelete.ConsumerGroupPrefix)
                .Single(acl => acl.UserId == _aclCreateDelete.ServiceAccountId);
        }
    }
}