using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tika.RestClient.Features.Topics.Models;

namespace Tika.RestClient.Features.Topics
{
    public interface ITopicsClient
    {
        Task<IEnumerable<string>> GetAllAsync(string clusterId = null);
        Task<TopicDescription> DescribeAsync(string topicName, string clusterId = null);

        Task CreateAsync(TopicCreate topicCreate, string clusterId = null);
        Task DeleteAsync(string topicName, string clusterId = null);
        
    }
}