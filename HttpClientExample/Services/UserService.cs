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

        // get all users from api
        public async Task<List<UserDto>> GetAllUsersFromApi()
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

        // get all users from the database
        public async Task<List<UserDto>> GetAllUsersFromDb()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                const string sql = @"
                    SELECT 
                        Id, Name, Username, Email, Phone, Website,
                        Street, Suite, City, Zipcode,
                        Lat, Lng,
                        CompanyName, CatchPhrase, Bs
                    FROM Users
                ";
                var users = await connection.QueryAsync(sql);

                // mapping to UserDto with nested objects
                return users.Select(u => new UserDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Username = u.Username,
                    Email = u.Email,
                    Phone = u.Phone,
                    Website = u.Website,
                    Address = new AddressDTO
                    {
                        Street = u.Street,
                        Suite = u.Suite,
                        City = u.City,
                        Zipcode = u.Zipcode,
                        Geo = new GeoDTO
                        {
                            Lat = u.Lat,
                            Lng = u.Lng
                        }
                    },
                    Company = new CompanyDTO
                    {
                        Name = u.CompanyName,
                        CatchPhrase = u.CatchPhrase,
                        Bs = u.Bs
                    }
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
                return [];
            }
        }

        public async Task InsertUsers(List<UserDto> users)
        {
            using var connection = new SqlConnection(_connectionString);

            var existingIds = (await connection.QueryAsync("SELECT Id FROM Users")).ToHashSet();

            string query = @"INSERT INTO USERS(Id, Name, Username, Email, Street, Suite, City, Zipcode, Lat, Lng, Phone, Website, CompanyName, CatchPhrase)
              VALUES (@Id, @Name, @Username, @Email, @Street, @Suite, @City, @Zipcode, @Lat, @Lng, @Phone, @Website, @CompanyName, @CatchPhrase)";

            foreach (var user in users)
            {
                if (existingIds.Contains(user.Id))
                    continue;
                var parameters = new
                {
                    user.Id,
                    user.Name,
                    user.Username,
                    user.Email,
                    Street = user.Address?.Street,
                    Suite = user.Address?.Suite,
                    City = user.Address?.City,
                    Zipcode = user.Address?.Zipcode,
                    Lat = user.Address?.Geo?.Lat,
                    Lng = user.Address?.Geo?.Lng,
                    user.Phone,
                    user.Website,
                    CompanyName = user.Company?.Name,
                    CatchPhrase = user.Company?.CatchPhrase
                };
                await connection.ExecuteAsync(query, parameters);
            }
        }

        // update users
        public async Task UpdateUsers(List<UserDto> users)
        {
            using var connection = new SqlConnection(_connectionString);
            string query = @"UPDATE USERS
                     SET Name = @Name,
                         Username = @Username,
                         Email = @Email,
                         Street = @Street,
                         Suite = @Suite,
                         City = @City,
                         Zipcode = @Zipcode,
                         Lat = @Lat,
                         Lng = @Lng,
                         Phone = @Phone,
                         Website = @Website,
                         CompanyName = @CompanyName,
                         CatchPhrase = @CatchPhrase,
                         Bs = @Bs
                     WHERE Id = @Id";

            foreach (var user in users)
            {
                if (user.Address is null || user.Address.Geo is null || user.Company is null)
                    continue; // or throw/log error if these are required

                await connection.ExecuteAsync(query, new
                {
                    user.Id,
                    user.Name,
                    user.Username,
                    user.Email,
                    Street = user.Address.Street,
                    Suite = user.Address.Suite,
                    City = user.Address.City,
                    Zipcode = user.Address.Zipcode,
                    Lat = user.Address.Geo.Lat,
                    Lng = user.Address.Geo.Lng,
                    user.Phone,
                    user.Website,
                    CompanyName = user.Company.Name,
                    CatchPhrase = user.Company.CatchPhrase,
                    Bs = user.Company.Bs
                });
            }
        }

        // delete user in db
        public async Task DeleteUser(int id)
        {
            using var connection = new SqlConnection(_connectionString);

            string query = @"DELETE FROM Users WHERE Id = @Id ";

            await connection.ExecuteAsync(query, new { Id = @id });
        }

        // check if objects are equal
        private bool UsersAreEqual(UserDto userA, UserDto userB)
        {
            if (userA == null || userB == null)
                return false;

            bool addressEqual = (userA.Address == null && userB.Address == null) ||
                                (userA.Address != null && userB.Address != null &&
                                 userA.Address.Street == userB.Address.Street &&
                                 userA.Address.Suite == userB.Address.Suite &&
                                 userA.Address.City == userB.Address.City &&
                                 userA.Address.Zipcode == userB.Address.Zipcode);

            return userA.Id == userB.Id
                && userA.Name == userB.Name
                && userA.Username == userB.Username
                && userA.Email == userB.Email
                && userA.Phone == userB.Phone
                && userA.Website == userB.Website
                && addressEqual;
        }

        // sync users
        public async Task SyncUsersWithApi()
        {
            List<UserDto> apiUsers = await GetAllUsersFromApi();
            List<UserDto> dbUsers = await GetAllUsersFromDb();

            var newUsers = new List<UserDto>();
            var updatedUsers = new List<UserDto>();

            // compare api user to db user
            foreach (var apiUser in apiUsers)
            {
                var dbUser = dbUsers.FirstOrDefault(c => c.Id == apiUser.Id);
                if (dbUser == null)
                {
                    newUsers.Add(apiUser);
                }
                else if (!UsersAreEqual(apiUser, dbUser))
                {
                    updatedUsers.Add(apiUser);
                }
            }
            var usersToDelete = dbUsers
                .Where(db => !apiUsers.Any(api => api.Id == db.Id)).ToList();

            // insert new users
            if (newUsers.Count > 0)
            {
                await InsertUsers(newUsers);
                Console.WriteLine($"Inserted {newUsers.Count} users");
            }

            // update changed users
            if (updatedUsers.Count > 0)
            {
                await UpdateUsers(updatedUsers);
                Console.WriteLine($"Updated {updatedUsers.Count} users");
            }

            // delete user
            if (usersToDelete.Count > 0)
            {
                foreach (var user in usersToDelete)
                {
                    await DeleteUser(user.Id);
                }
                Console.WriteLine($"Deleted {usersToDelete.Count} {(usersToDelete.Count == 1 ? "user" : "users")}");
            }

            // log if no changes detected
            if (newUsers.Count == 0 && updatedUsers.Count == 0 && usersToDelete.Count == 0)
            {
                Console.WriteLine("No changes detected in users.");
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
    }
}