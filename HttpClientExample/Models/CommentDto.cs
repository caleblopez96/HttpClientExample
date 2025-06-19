using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HttpClientExample.Models
{

    //    {
    //    "postId": 1,
    //    "id": 5,
    //    "name": "vero eaque aliquid doloribus et culpa",
    //    "email": "Hayden@althea.biz",
    //    "body": "harum non quasi et ratione\ntempore iure ex voluptates in ratione\nharum architecto fugit inventore cupiditate\nvoluptates magni quo et"
    //},
    public class CommentDto
    {
        [JsonPropertyName("postId")]
        public required int PostId { get; set; }

        [JsonPropertyName("id")]
        public required int Id { get; set; }

        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("email")]
        public required string Email { get; set; }

        [JsonPropertyName("body")]
        public required string Body { get; set; }
    }
}
