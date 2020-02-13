using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Tika.RestClient.Features.ApiKeys.Models;

namespace Tika.RestClient.Features.ApiKeys
{
    public class ApiKeysClient : IApiKeysClient
    {
        private const string APIKEYS_ROUTE = "/api-keys";
        private readonly HttpClient _httpClient;

        public ApiKeysClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public async Task<IEnumerable<ApiKey>> GetAllAsync()
        {
            var httpResponseMessage = await _httpClient.GetAsync(
                new Uri(APIKEYS_ROUTE, UriKind.Relative)
            );
            
            var serviceAccounts = await Utilities.Parse<IEnumerable<ApiKey>>(httpResponseMessage);

            return serviceAccounts;
        }

        public async Task<ApiKey> CreateAsync(ApiKeyCreate apiKeyCreate)
        {
            var payload = JsonConvert.SerializeObject(apiKeyCreate);

            var content = new StringContent(
                payload,
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync(
                new Uri(APIKEYS_ROUTE, UriKind.Relative),
                content
            );

            var apiKey = await Utilities.Parse<ApiKey>(response);

            return apiKey;
        }

        public async Task DeleteAsync(string key)
        {
            var httpResponseMessage = await _httpClient.DeleteAsync(
                new Uri(APIKEYS_ROUTE + "/" + key, UriKind.Relative)
            );

            httpResponseMessage.EnsureSuccessStatusCode();
        }
    }
}