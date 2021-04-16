using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Tika.RestClient
{
    public class Utilities
    {
        public static async Task<T> Parse<T>(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<T>(content);
            return data;
        }

        public static string MakeUrl(ClientOptions clientOptions, string path, string cluster = null)
        {
            if (clientOptions.TIKA_ENABLE_MULTI_CLUSTER)
            {
                return $"{clientOptions.TIKA_MULTI_CLUSTER_HOSTNAME_PREFIX}-{cluster}:3000{path}";
            }
            else
            {
                return $"{clientOptions.TIKA_API_ENDPOINT}{path}";
            }
        }

    }

    public class UrlResult
    {
        public string Url { get; set; }
        public UriKind UriKind { get; set; }
    }
}