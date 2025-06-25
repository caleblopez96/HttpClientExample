using System.Text.Json.Serialization;

namespace HttpClientExample.Models
{
    public class PhotoDto
    {
        [JsonPropertyName("albumId")]
        public required int AlbumId { get; set; }

        [JsonPropertyName("id")]
        public required int Id { get; set; }

        [JsonPropertyName("title")]
        public required string Title { get; set; }

        [JsonPropertyName("url")]
        public required string Url { get; set; }

        [JsonPropertyName("thumbnailUrl")]
        public required string ThumbnailUrl { get; set; }
    }
}
