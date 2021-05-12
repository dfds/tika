using System;
using Microsoft.Extensions.Configuration;

namespace Tika.RestClient
{
    public class ClientOptions
    {
        public string TIKA_API_ENDPOINT { get; set; }
        public bool TIKA_ENABLE_MULTI_CLUSTER { get; set; }
        public string TIKA_MULTI_CLUSTER_HOSTNAME_PREFIX { get; set; }
        
        public ClientOptions() {}

        public ClientOptions(IConfiguration conf)
        {
            // Main config
            // Will be ignored if TIKA_ENABLE_MULTI_CLUSTER is enabled
            TIKA_API_ENDPOINT = ConfToString(conf, "TIKA_API_ENDPOINT", null);
            
            // Enable features
            TIKA_ENABLE_MULTI_CLUSTER = ConfToBool(conf, "TIKA_ENABLE_MULTI_CLUSTER", false);
            
            // Multi-cluster config
            TIKA_MULTI_CLUSTER_HOSTNAME_PREFIX = ConfToString(conf, "TIKA_MULTI_CLUSTER_HOSTNAME_PREFIX", "http://tika-");
        }
        
        private static bool ConfToBool(IConfiguration conf, string key, bool defaultValue)
        {
            var val = conf[key];

            if (bool.TryParse(val, out bool result))
            {
                return result;
            }

            return defaultValue;
        }

        private static string ConfToString(IConfiguration conf, string key, string defaultValue)
        {
            var val = conf[key];

            if (String.IsNullOrEmpty(val))
            {
                return defaultValue;
            }

            return val;
        }
    }
}