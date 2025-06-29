using System.Net.Http.Json;
using HttpClientExample.Models;
using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace HttpClientExample.Services
{
    public class UserService(HttpClient client, IConfiguration configuration)
    {
        private readonly HttpClient _client = client;
        private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection");
        private readonly string _baseUrl = "https://jsonplaceholder.typicode.com";
        private readonly string _endPoint = "/users";

        public async Task<List<UserDto>> GetAllUsers()
        {
            try
            {
                // Fixed: changed *client and *baseUrl to _client and _baseUrl
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

        // Dapper example: fetch all users from the database
        public async Task<List<UserDto>> GetAllUsersFromDbAsync()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                const string sql = @"
                    SELECT Id, Name, Username, Email, Phone
                    FROM Users
                ";
                var users = await connection.QueryAsync<UserDto>(sql);
                return users.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
                return [];
            }
        }
    }
}