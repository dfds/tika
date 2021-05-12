namespace Tika.RestClient.Features.ApiKeys.Models
{
    public class ApiKey
    {
        public string Key { get; set; }
        public string Secret { get; set; }
        public string Description { get; set; }
        public string Owner { get; set; }
        public string Resource { get; set; }
    }
}