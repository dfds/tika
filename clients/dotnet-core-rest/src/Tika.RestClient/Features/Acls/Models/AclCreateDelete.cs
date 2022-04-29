namespace Tika.RestClient.Features.Acls.Models
{
    public class AclCreateDelete
    {
        public string ServiceAccountId { get; set; }
        public bool Allow { get; set; }
        public string Operation { get; set; }
        public string TopicPrefix { get; set; }
        public string ConsumerGroupPrefix { get; set; }
        
        public AclCreateDelete() {}

        public AclCreateDelete(string serviceAccountId, bool allow, string operation, string topicPrefix = "", string consumerGroupPrefix = "")
        {
            ServiceAccountId = serviceAccountId;
            Allow = allow;
            Operation = operation;
            TopicPrefix = topicPrefix;
            ConsumerGroupPrefix = consumerGroupPrefix;
        }
    }
}