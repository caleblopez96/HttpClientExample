using HttpClientExample.Models;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace HttpClientExample.Services
{
    public class CommentService
    {
        readonly HttpClient _client;
        readonly string _baseUrl = "https://jsonplaceholder.typicode.com";

        public CommentService(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<CommentDto>> GetAllComments()
        {
            string _endpoint = "/comments";
            try
            {
                List<CommentDto>? comments = await _client.GetFromJsonAsync<List<CommentDto>>(_baseUrl + _endpoint);
                return comments ?? new List<CommentDto>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Error: {ex.Message}");
                return new List<CommentDto>();
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Json Error: {ex.Message}");
                return new List<CommentDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<CommentDto>();
            }
        }

        public async Task<CommentDto> GetCommentByCommentId(int commentId)
        {
            string endpoint = $"/comments/{commentId}";
            try
            {
                CommentDto? comment = await _client.GetFromJsonAsync<CommentDto>(_baseUrl + endpoint);
                return comment ?? new CommentDto();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Error: {ex.Message}");
                return new CommentDto();
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON Error: {ex.Message}");
                return new CommentDto();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new CommentDto();
            }
        }
    }
}
