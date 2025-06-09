using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HttpClientExample.Models.Practice
{
    internal class TicketResponse
    {
        public class ApiResponse
        {
            [JsonPropertyName("success")]
            public required bool Success { get; set; }

            [JsonPropertyName("data")]
            public List<Data>? Data { get; set; }
        }

        public class Data
        {
            [JsonPropertyName("ticket_id")]
            public required int TicketId { get; set; }

            [JsonPropertyName("subject")]
            public string? Subject { get; set; }

            [JsonPropertyName("created_at")]
            public string? CreatedAt { get; set; }

            [JsonPropertyName("status")]
            public Status? Status { get; set; }

            [JsonPropertyName("user")]
            public User? User { get; set; }

            [JsonPropertyName("comments")]
            public List<Comments>? Comments { get; set; }
        }

        public class Status
        {
            [JsonPropertyName("code")]
            public string? Code { get; set; }

            [JsonPropertyName("description")]
            public string? Description { get; set; }
        }

        public class User
        {
            [JsonPropertyName("user_id")]
            public required int UserId { get; set; }

            [JsonPropertyName("name")]
            public Name? Name { get; set; }

            [JsonPropertyName("email")]
            public string? Email { get; set; }
        }

        public class Name
        {
            [JsonPropertyName("first")]
            public string? FirstName { get; set; }

            [JsonPropertyName("last")]
            public string? LastName { get; set; }
        }

        public class Comments
        {
            [JsonPropertyName("comment_id")]
            public required int CommentId { get; set; }

            [JsonPropertyName("author")]
            public string? Author { get; set; }

            [JsonPropertyName("body")]
            public string? Body { get; set; }

            [JsonPropertyName("posted_at")]
            public string? PostedAt { get; set; }
        }
    }
}
