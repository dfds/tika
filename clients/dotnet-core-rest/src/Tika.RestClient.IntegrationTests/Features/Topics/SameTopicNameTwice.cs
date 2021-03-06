using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tika.RestClient.Features.Topics.Exceptions;
using Tika.RestClient.Features.Topics.Models;
using Tika.RestClient.IntegrationTests.Features.Factories;
using Xunit;

namespace Tika.RestClient.IntegrationTests.Features.Topics
{
    public class SameTopicNameTwice : IDisposable
    {
        private IRestClient _client;
        private string _topicName;
        private TopicAlreadyExistsException _topicAlreadyExistsException;
        private IEnumerable<string> _topicNames;

        [Fact]
        public async Task SameTopicNameTwiceScenario()
        {
                  Given_a_topic_client();
            await And_a_single_topic();
            await When_a_topic_with_the_same_name_is_added();
            await And_we_get_topic_names();
                  Then_we_only_get_one_topic_with_our_name();
                  
        }


        private void Given_a_topic_client()
        {
            _client = LocalhostRestClient.Create();
        }

        private async Task And_a_single_topic()
        {
            _topicName = "Integration-test-" + Guid.NewGuid();
            var topicCreate = TopicCreate.Create(
                name: _topicName,
                partitionCount: 1
            );

            await _client.Topics.CreateAsync(topicCreate);
        }


        private async Task When_a_topic_with_the_same_name_is_added()
        {
            var topicCreate = TopicCreate.Create(
                name: _topicName,
                partitionCount: 1
            );
                
                
            await _client.Topics.CreateAsync(topicCreate);
        }

        private async Task And_we_get_topic_names()
        {
            _topicNames = await _client.Topics.GetAllAsync();
        }
        private void Then_we_only_get_one_topic_with_our_name()
        {
            Assert.Single(_topicNames, _topicName);
        }

        public void Dispose()
        {
            var task = _client.Topics.DeleteAsync(_topicName);
            task.Wait();
        }
    }
}