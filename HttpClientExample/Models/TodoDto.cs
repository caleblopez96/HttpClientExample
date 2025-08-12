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
        [JsonPropertyName("UserId")]
        public required int UserId { get; set; }

        [JsonPropertyName("Id")]
        public required int Id { get; set; }

        [JsonPropertyName("Title")]
        public required string Title { get; set; }

        [JsonPropertyName("Completed")]
        public required bool Completed { get; set; }
    }
}
