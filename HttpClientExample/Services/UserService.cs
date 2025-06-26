using System.Net.Http.Json;
using HttpClientExample.Models;

namespace HttpClientExample.Services
{
    public class UserService(HttpClient client)
    {
        private readonly HttpClient _client = client;
        private readonly string _baseUrl = "https://jsonplaceholder.typicode.com";
        private readonly string _endPoint = "/users";

        public async Task<List<UserDto>> GetAllUsers()
        {
            try
            {
                var users = await _client.GetFromJsonAsync<List<UserDto>>(_baseUrl + _endPoint);
                return users ?? new List<UserDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting users: {ex.Message}");
                return new List<UserDto>();
            }
        }
    }
}
