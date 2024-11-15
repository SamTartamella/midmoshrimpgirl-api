using System.Text.Json.Serialization;

namespace midmoshrimpgirl_api.Models.Responses
{
    public class ProductResponse
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("imageLink")]
        public string ImageLink { get; set; }
    }
}
