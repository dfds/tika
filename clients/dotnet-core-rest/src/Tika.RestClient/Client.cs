using System.Net.Http;
using Microsoft.Extensions.Options;
using Tika.RestClient.Features.Acls;
using Tika.RestClient.Features.ApiKeys;
using Tika.RestClient.Features.ServiceAccounts;
using Tika.RestClient.Features.Topics;

namespace Tika.RestClient
{
    internal class Client : IRestClient
    {
        private HttpClient _httpClient;
        private ClientOptions _clientOptions;
        public ITopicsClient Topics { get; }
        public IServiceAccountsClient ServiceAccounts { get; }
        public IApiKeysClient ApiKeys { get; }
        public IAclsClient Acls { get; }

        public Client(HttpClient httpClient, ClientOptions clientOptions)
        {
            _httpClient = httpClient;
            Topics = new TopicsClient(httpClient, clientOptions);
            ServiceAccounts = new ServiceAccountsClient(httpClient, clientOptions);
            ApiKeys = new ApiKeysClient(httpClient, clientOptions);
            Acls = new AclsClient(httpClient, clientOptions);
        }
        
        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}