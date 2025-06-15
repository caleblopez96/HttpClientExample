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


        public async Task<List<PostDTO>> GetAllPostAsync()
        {
            string endPoint = "/posts";
            try
            {
                List<PostDTO>? posts = await _client.GetFromJsonAsync<List<PostDTO>>(baseUrl + endPoint);
                return posts ?? new List<PostDTO>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Error: {ex.Message}");
                return new List<PostDTO>();
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON Error: {ex.Message}");
                return new List<PostDTO>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<PostDTO>();
            }
        }

        public async Task<PostDTO?> GetPostByIdAsync(int postId)
        {
            string endPoint = $"/posts/{postId}";
            try
            {
                var post = await _client.GetFromJsonAsync<PostDTO>(baseUrl + endPoint);
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
