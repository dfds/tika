using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly ClientOptions _clientOptions;

        public ApiKeysClient(HttpClient httpClient, ClientOptions clientOptions)
        {
            _httpClient = httpClient;
            _clientOptions = clientOptions;
        }
        
        public async Task<IEnumerable<ApiKey>> GetAllAsync(string clusterId = null)
        {
            
            var httpResponseMessage = await _httpClient.GetAsync(
                new Uri(Utilities.MakeUrl(_clientOptions, APIKEYS_ROUTE, clusterId), UriKind.Absolute)
            );
            
            var apiKeys = await Utilities.Parse<IEnumerable<ApiKey>>(httpResponseMessage);
            apiKeys = apiKeys.Where(key => key.Resource.ToLower().Equals(clusterId.ToLower()));

            return apiKeys;
        }

        public async Task<ApiKey> CreateAsync(ApiKeyCreate apiKeyCreate, string clusterId = null)
        {
            var payload = JsonConvert.SerializeObject(new
            {
                serviceAccountId = apiKeyCreate.ServiceAccountId,
                description = apiKeyCreate.Description
            });

            var content = new StringContent(
                payload,
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync(
                new Uri(Utilities.MakeUrl(_clientOptions, APIKEYS_ROUTE, clusterId), UriKind.Absolute),
                content
            );

            var apiKey = await Utilities.Parse<ApiKey>(response);

            return apiKey;
        }

        public async Task DeleteAsync(string key, string clusterId = null)
        {
            
            var httpResponseMessage = await _httpClient.DeleteAsync(
                new Uri(Utilities.MakeUrl(_clientOptions, APIKEYS_ROUTE + "/" + key, clusterId), UriKind.Absolute)
            );

            httpResponseMessage.EnsureSuccessStatusCode();
        }
    }
}