using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HttpClientExample.Models
{
    public class TodoDto
    {
        [JsonPropertyName("userId")]
        public required int UserId { get; set; }

        [JsonPropertyName("id")]
        public required int Id { get; set; }

        [JsonPropertyName("title")]
        public required string Title { get; set; }

        [JsonPropertyName("completed")]
        public required bool Status { get; set; }
    }
}
