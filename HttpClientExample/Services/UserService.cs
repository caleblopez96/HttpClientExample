using System.Net.Http.Json;
using HttpClientExample.Models;

namespace HttpClientExample.Services
{
    public class UserService(HttpClient client)
    {
        // using primary constructor
        private readonly HttpClient _client = client;
        private readonly string _baseUrl = "https://jsonplaceholder.typicode.com";
        private readonly string _endPoint = "/users";

        // using primary constructor instead
        //public UserService(HttpClient client)
        //{
        //    _client = client;
        //}

        public async Task<List<UserDto>> GetAllUsers()
        {
            try
            {
                var users = await _client.GetFromJsonAsync<List<UserDto>>(_baseUrl + _endPoint);
                return users ?? [];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting users: {ex.Message}");
                return [];
            }
        }

        public async Task<UserDto> GetUserById(int id)
        {
            string endpoint = $"{_baseUrl}/users/{id}";
            try
            {
                var user = await _client.GetFromJsonAsync<UserDto>(endpoint);
                return user ?? new UserDto
                {
                    Id = 0,
                    Name = string.Empty,
                    Username = string.Empty,
                    Email = string.Empty,
                    Phone = string.Empty
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new UserDto
                {
                    Id = 0,
                    Name = string.Empty,
                    Username = string.Empty,
                    Email = string.Empty,
                    Phone = string.Empty
                };
            }
        }
    }
}
