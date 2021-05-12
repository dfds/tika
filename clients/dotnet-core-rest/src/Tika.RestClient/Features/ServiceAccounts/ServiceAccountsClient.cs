using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Tika.RestClient.Features.ServiceAccounts.Models;

namespace Tika.RestClient.Features.ServiceAccounts
{
    public class ServiceAccountsClient : IServiceAccountsClient
    {
        private const string SERVICE_ACCOUNTS_ROUTE = "/service-accounts";
        private readonly HttpClient _httpClient;
        private readonly ClientOptions _clientOptions;

        public ServiceAccountsClient(HttpClient httpClient, ClientOptions clientOptions)
        {
            _httpClient = httpClient;
            _clientOptions = clientOptions;
        }
        
        public async Task<IEnumerable<ServiceAccount>> GetAllAsync(string clusterId = null)
        {
            
            var httpResponseMessage = await _httpClient.GetAsync(
                new Uri(Utilities.MakeUrl(_clientOptions, SERVICE_ACCOUNTS_ROUTE, clusterId), UriKind.Absolute)
            );
            
            var serviceAccounts = await Utilities.Parse<IEnumerable<ServiceAccount>>(httpResponseMessage);

            return serviceAccounts;
        }

        public async Task<ServiceAccount> CreateAsync(ServiceAccountCreateCommand serviceAccountCreateCommand, string clusterId = null)
        {
            var payload = JsonConvert.SerializeObject(serviceAccountCreateCommand);

            var content = new StringContent(
                payload,
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync(
                new Uri(Utilities.MakeUrl(_clientOptions, SERVICE_ACCOUNTS_ROUTE, clusterId), UriKind.Absolute),
                content
            );

            var serviceAccount = await Utilities.Parse<ServiceAccount>(response);

            return serviceAccount;
        }

        public async Task DeleteAsync(string id, string clusterId = null)
        {
            
            var httpResponseMessage = await _httpClient.DeleteAsync(
                new Uri(Utilities.MakeUrl(_clientOptions, SERVICE_ACCOUNTS_ROUTE + "/" + id, clusterId), UriKind.Absolute)
            );

            httpResponseMessage.EnsureSuccessStatusCode();
        }
    }
}