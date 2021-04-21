using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tika.RestClient.Features.Topics.Models;
using Newtonsoft.Json;
using Tika.RestClient.Features.Topics.Exceptions;

namespace Tika.RestClient.Features.Topics
{
    internal class TopicsClient : ITopicsClient
    {
        private const string TOPICS_ROUTE = "/topics";
        private readonly HttpClient _httpClient;
        private readonly ClientOptions _clientOptions;

        public TopicsClient(HttpClient httpClient, ClientOptions clientOptions)
        {
            _httpClient = httpClient;
            _clientOptions = clientOptions;
        }

        public async Task<IEnumerable<string>> GetAllAsync(string clusterId = null)
        {
            Console.WriteLine(Utilities.MakeUrl(_clientOptions, TOPICS_ROUTE, clusterId).ToString());
            var httpResponseMessage = await _httpClient.GetAsync(
                new Uri(Utilities.MakeUrl(_clientOptions, TOPICS_ROUTE, clusterId), UriKind.Absolute)
            );
            
            var topics = await Utilities.Parse<IEnumerable<string>>(httpResponseMessage);

            return topics;
        }

        /// <exception cref="Tika.RestClient.Features.Topics.Exceptions.TopicAlreadyExistsException">Thrown when topic with given name already exists</exception>
        public async Task CreateAsync(TopicCreate topicCreate, string clusterId = null)
        {
            var payload = JsonConvert.SerializeObject(topicCreate);

            var content = new StringContent(
                payload,
                Encoding.UTF8,
                "application/json"
            );

            var res = await _httpClient.PostAsync(
                new Uri(Utilities.MakeUrl(_clientOptions, TOPICS_ROUTE, clusterId), UriKind.Absolute),
                content
            );

            if (res.StatusCode == HttpStatusCode.Conflict)
            {
                var rawText = await res.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<TikaGenericError>(rawText);
                if (responseData.ErrName == "CcloudTopicAlreadyExists")
                {
                    throw new TopicAlreadyExistsException(responseData.ErrMessage);
                }
            }
        }

        public async Task DeleteAsync(string topicName, string clusterId = null)
        {
            
            var httpResponseMessage = await _httpClient.DeleteAsync(
                new Uri(Utilities.MakeUrl(_clientOptions, TOPICS_ROUTE + "/" + topicName, clusterId), UriKind.Absolute)
            );

            httpResponseMessage.EnsureSuccessStatusCode();
        }

        public async Task<TopicDescription> DescribeAsync(string topicName, string clusterId = null)
        {
            var httpResponseMessage = await _httpClient.GetAsync(
                new Uri(Utilities.MakeUrl(_clientOptions, TOPICS_ROUTE + "/" + topicName, clusterId), UriKind.Absolute)
            );
            
            var topicDescription = await Utilities.Parse<TopicDescription>(httpResponseMessage);

            return topicDescription;
        }
    }

    internal class TikaGenericError
    {
        public string ErrName { get; set; }
        public string ErrMessage { get; set; }
    }
}