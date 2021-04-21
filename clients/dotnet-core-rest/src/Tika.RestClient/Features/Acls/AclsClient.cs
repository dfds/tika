using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Tika.RestClient.Features.Acls.Models;

namespace Tika.RestClient.Features.Acls
{
    public class AclsClient : IAclsClient
    {
        private const string ACLS_ROUTE = "/access-control-lists";
        private readonly HttpClient _httpClient;
        private readonly ClientOptions _clientOptions;

        public AclsClient(HttpClient httpClient, ClientOptions options)
        {
            _httpClient = httpClient;
            _clientOptions = options;
        }
        
        public async Task<IEnumerable<Acl>> GetAllAsync(string clusterId = null)
        {
            Console.WriteLine(Utilities.MakeUrl(_clientOptions, ACLS_ROUTE, clusterId).ToString());
            var httpResponseMessage = await _httpClient.GetAsync(
                new Uri(Utilities.MakeUrl(_clientOptions, ACLS_ROUTE, clusterId), UriKind.Absolute)
            );
            
            var acls = await Utilities.Parse<IEnumerable<Acl>>(httpResponseMessage);

            return acls;
        }

        public async Task CreateAsync(AclCreateDelete aclCreateDelete, string clusterId = null)
        {
            var payload = JsonConvert.SerializeObject(new
            {
                serviceAccountId = aclCreateDelete.ServiceAccountId,
                allow = aclCreateDelete.Allow,
                operation = aclCreateDelete.Operation,
                topicPrefix = aclCreateDelete.TopicPrefix,
                consumerGroupPrefix = aclCreateDelete.ConsumerGroupPrefix
            });

            var content = new StringContent(
                payload,
                Encoding.UTF8,
                "application/json"
            );

            await _httpClient.PostAsync(
                new Uri(Utilities.MakeUrl(_clientOptions, ACLS_ROUTE, clusterId), UriKind.Absolute),
                content
            );
        }

        public async Task DeleteAsync(AclCreateDelete aclDelete, string clusterId = null)
        {
            var payload = JsonConvert.SerializeObject(new
            {
                serviceAccountId = aclDelete.ServiceAccountId,
                allow = aclDelete.Allow,
                operation = aclDelete.Operation,
                topicPrefix = aclDelete.TopicPrefix,
                consumerGroupPrefix = aclDelete.ConsumerGroupPrefix
            });

            var content = new StringContent(
                payload,
                Encoding.UTF8,
                "application/json"
            );

            var httpResponseMessage = await _httpClient.PostAsync(
                new Uri(Utilities.MakeUrl(_clientOptions, ACLS_ROUTE + "/delete", clusterId), UriKind.Absolute), 
                content
            );

            httpResponseMessage.EnsureSuccessStatusCode();
        }
    }
}