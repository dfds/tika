using System;
using System.Net.Http;
using Microsoft.Extensions.Options;

namespace Tika.RestClient.Factories
{
    public static class RestClientFactory
    {
        public static IRestClient Create(HttpClient httpClient, ClientOptions options, string cluster = "")
        {
            return new Client(httpClient, options);
        }

        public static IRestClient CreateFromConfiguration(HttpClient httpClient, IOptions<ClientOptions> options, string cluster = "")
        {
            if (options.Value?.TIKA_API_ENDPOINT == null && options.Value?.TIKA_ENABLE_MULTI_CLUSTER != true)
            {
                throw new TikaRestClientInvalidConfigurationException("TIKA_API_ENDPOINT");
            }

            if (options.Value?.TIKA_ENABLE_MULTI_CLUSTER == false)
            {
                httpClient.BaseAddress = new Uri(options.Value?.TIKA_API_ENDPOINT);
            }
            else
            {
                httpClient.BaseAddress = new Uri($"{options.Value?.TIKA_MULTI_CLUSTER_HOSTNAME_PREFIX}-{cluster}:3000");
            }
            
            return new Client(httpClient, options.Value);
        }
    }

    public class TikaRestClientInvalidConfigurationException : Exception
    {
        public TikaRestClientInvalidConfigurationException() : base("RestClientFactory was unable to find the necessary configuration to create a RestClient. Please refer to the configuration section of Tika.RestClient's README.")
        {
            
        }

        public TikaRestClientInvalidConfigurationException(string message) : base($"RestClientFactory was unable to find the necessary configuration for '{message}' to create a RestClient. Please refer to the configuration section of Tika.RestClient's README.")
        {
            
        }
        
        public TikaRestClientInvalidConfigurationException(string message, Exception inner) : base($"RestClientFactory was unable to find the necessary configuration for '{message}' to create a RestClient. Please refer to the configuration section of Tika.RestClient's README.", inner)
        {
            
        }
    }
}
