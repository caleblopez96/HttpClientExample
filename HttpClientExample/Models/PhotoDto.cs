using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HttpClientExample.Models
{
    //{
    //    "albumId": 1,
    //    "id": 1,
    //    "title": "accusamus beatae ad facilis cum similique qui sunt",
    //    "url": "https://via.placeholder.com/600/92c952",
    //    "thumbnailUrl": "https://via.placeholder.com/150/92c952"
    //}
    public class PhotoDto
    {
        [JsonPropertyName("albumId")]
        public int? AlbumId { get; set; }

        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("thumbnailUrl")]
        public string? ThumbnailUrl { get; set; }
    }
}
