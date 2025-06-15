using System.Text.Json.Serialization;


namespace HttpClientExample.Models
{
    public class PostDTO
    {
        [JsonPropertyName("userId")]
        public required int UserId { get; set; }

        [JsonPropertyName("id")]
        public required int Id { get; set; }

        [JsonPropertyName("title")]
        public required string Title { get; set; }

        [JsonPropertyName("body")]
        public required string Body { get; set; }
    }
}
