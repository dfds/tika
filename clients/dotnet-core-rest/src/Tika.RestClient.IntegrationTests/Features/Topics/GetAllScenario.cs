using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tika.RestClient.Features.Topics.Models;
using Tika.RestClient.IntegrationTests.Features.Factories;
using Xunit;

namespace Tika.RestClient.IntegrationTests.Features.Topics
{
    public class GetAllScenario : IDisposable
    {
        private IRestClient _client;
        private TopicCreate _topicCreateCommand;
        private IEnumerable<string> _returnedTopics;

        [Fact]
        public async Task GetAllWIthSingleTopicScenario()
        {
                  Given_a_topic_client();
            await And_a_single_topic();
            await When_GetAll_is_called();
                  Then_the_topic_is_returned();
        }

      
        private void Given_a_topic_client()
        {
            _client = LocalhostRestClient.Create();
        }

        private async Task And_a_single_topic()
        {
            using var client = LocalhostRestClient.Create();
            _topicCreateCommand = TopicCreate.Create(
                name: Guid.NewGuid().ToString(),
                partitionCount: 1
            );
            
            await client.Topics.CreateAsync(_topicCreateCommand);
        }

        private async Task When_GetAll_is_called()
        {
            _returnedTopics = await _client.Topics.GetAllAsync();
        }

        private void Then_the_topic_is_returned()
        {
            _returnedTopics.Single(t => t == _topicCreateCommand.Name);
        }

        public void Dispose()
        {
            var task = _client.Topics.DeleteAsync(_topicCreateCommand.Name);
            task.Wait();
        }
    }
}