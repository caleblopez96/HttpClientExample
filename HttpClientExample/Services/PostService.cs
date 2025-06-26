using System.ComponentModel;
using System.Net.Http.Json;
using System.Text.Json;
using HttpClientExample.Models;

namespace HttpClientExample.Services
{
    public class PostService(HttpClient client)
    {
        // using primary constructor
        readonly HttpClient _client = client;
        readonly string baseUrl = "https://jsonplaceholder.typicode.com";

        // using primary constructor instead
        //public PostService(HttpClient client)
        //{
        //    _client = client;
        //}


        public async Task<List<PostDto>> GetAllPostAsync()
        {
            string endPoint = "/posts";
            try
            {
                List<PostDto>? posts = await _client.GetFromJsonAsync<List<PostDto>>(baseUrl + endPoint);
                //return posts ?? new List<PostDto>();
                return posts ?? [];
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Error: {ex.Message}");
                //return new List<PostDto>();
                return [];
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON Error: {ex.Message}");
                //return new List<PostDto>();
                return [];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                //return new List<PostDto>();
                return [];
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
