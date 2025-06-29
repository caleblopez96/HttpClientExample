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
        // using primary constructor instead
        private readonly HttpClient _client = client;
        private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection");
        private readonly string _baseUrl = "https://jsonplaceholder.typicode.com";
        private readonly string _usersEndPoint = "/users";

        public async Task<List<UserDto>> GetAllUsers()
        {
            try
            {
                var users = await _client.GetFromJsonAsync<List<UserDto>>(_baseUrl + _usersEndPoint);
                return users ?? [];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting users: {ex.Message}");
                return [];
            }
        }

        // get user from api by id
        public async Task<UserDto> GetUserById(int id)
        { 
            string endpoint = $"{_baseUrl}/{_usersEndPoint}/{id}";
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

        // get all users from the database
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

        // compare results of api and db
        public async Task CompareUsersFromDbAndApi()
        {
            // get users from the db and api
            var dbUsers = await GetAllUsersFromDbAsync();
            var apiUsers = await GetAllUsers();

            // get ids of db and api users and put them into a hashset
            var dbUserIds = dbUsers.Select(u => u.Id).ToHashSet();
            var apiUserIds = apiUsers.Select(u => u.Id).ToHashSet();

            // filter out the user ids that exist
            var inDbNotApi = dbUsers.Where(u => !apiUserIds.Contains(u.Id)).ToList();
            var inApiNotDb = apiUsers.Where(u => !dbUserIds.Contains(u.Id)).ToList();

            Console.WriteLine($"Users in DB but not in API: {inDbNotApi.Count}");
            foreach (var user in inDbNotApi)
            {
                Console.WriteLine($"{user.Id}: {user.Name} ({user.Email})");
            }

            Console.WriteLine($"Users in API but not in DB: {inApiNotDb.Count}");
            foreach (var user in inApiNotDb)
            {
                Console.WriteLine($"{user.Id}: {user.Name} ({user.Email})");
            }

            // insert the user into db if they dont exist
            if (inApiNotDb.Any())
            {
                using var connection = new SqlConnection(_connectionString);
                const string insertQuery = @"
                    INSERT INTO Users (Id, Name, Username, Email, Phone)
                    VALUES (@Id, @Name, @Username, @Email, @Phone);
                ";

                foreach (var user in inApiNotDb)
                {
                    var parameters = new
                    {
                        user.Id,
                        user.Name,
                        user.Username,
                        user.Email,
                        user.Phone
                    };
                    await connection.ExecuteAsync(insertQuery, parameters);
                    Console.WriteLine($"Inserted user {user.Id}: {user.Name} ({user.Email}) into DB.");
                }
            }


            var commonIds = dbUserIds.Intersect(apiUserIds);
            foreach (var id in commonIds)
            {
                var dbUser = dbUsers.First(u => u.Id == id);
                var apiUser = apiUsers.First(u => u.Id == id);
                if (!dbUser.Name.Equals(apiUser.Name, StringComparison.OrdinalIgnoreCase) ||
                    !dbUser.Email.Equals(apiUser.Email, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"Difference for user {id}:");
                    Console.WriteLine($"  DB:  {dbUser.Name}, {dbUser.Email}");
                    Console.WriteLine($"  API: {apiUser.Name}, {apiUser.Email}");
                }
            }
        }
    }
}