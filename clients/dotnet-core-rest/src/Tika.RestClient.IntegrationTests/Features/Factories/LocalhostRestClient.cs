using System;
using System.Net.Http;
using Tika.RestClient.Factories;

namespace Tika.RestClient.IntegrationTests.Features.Factories
{
    public static class LocalhostRestClient
    {
        public static IRestClient Create()
        {
            var httpClient = new HttpClient {BaseAddress = new Uri("http://localhost:3000/")};
            var clientOptions = new ClientOptions
            {
                TIKA_ENABLE_MULTI_CLUSTER = false,
                TIKA_API_ENDPOINT = "http://localhost:3000",
                TIKA_MULTI_CLUSTER_HOSTNAME_PREFIX = null
            };

            var restClient = RestClientFactory.Create(httpClient, clientOptions);

            return restClient;
        }
    }
}