namespace Tika.RestClient.Features.Acls.Models
{
    public class Acl
    {
        public string UserId { get; set; }
        public string ServiceAccountId { get; set; }
        public string Permission { get; set; }
        public string Resource { get; set; }
        public string Operation { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string TopicPrefix { get; set; }
        public string ConsumerGroupPrefix { get; set; }
    }
}