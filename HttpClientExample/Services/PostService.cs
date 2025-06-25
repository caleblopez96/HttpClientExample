using System.ComponentModel;
using System.Net.Http.Json;
using System.Text.Json;
using HttpClientExample.Models;

namespace HttpClientExample.Services
{
    public class PostService
    {
        readonly HttpClient _client = new();
        string baseUrl = "https://jsonplaceholder.typicode.com";

        public PostService(HttpClient client)
        {
            _client = client;
        }


        public async Task<List<PostDto>> GetAllPostAsync()
        {
            string endPoint = "/posts";
            try
            {
                List<PostDto>? posts = await _client.GetFromJsonAsync<List<PostDto>>(baseUrl + endPoint);
                return posts ?? new List<PostDto>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Error: {ex.Message}");
                return new List<PostDto>();
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON Error: {ex.Message}");
                return new List<PostDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<PostDto>();
            }
        }

        public async Task<PostDto?> GetPostByIdAsync(int postId)
        {
            string endPoint = $"/posts/{postId}";
            try
            {
                var post = await _client.GetFromJsonAsync<PostDto>(baseUrl + endPoint);
                return post;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
