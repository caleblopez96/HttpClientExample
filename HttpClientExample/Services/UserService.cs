using System.Net.Http.Json;
using HttpClientExample.Models;

namespace HttpClientExample.Services
{
    public class UserService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl = "https://jsonplaceholder.typicode.com";
        private readonly string _endPoint = "/users";

        public UserService(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<UserDTO>> GetAllUsers()
        {
            try
            {
                var users = await _client.GetFromJsonAsync<List<UserDTO>>(_baseUrl + _endPoint);
                return users ?? new List<UserDTO>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting users: {ex.Message}");
                return new List<UserDTO>();
            }
        }
    }
}
