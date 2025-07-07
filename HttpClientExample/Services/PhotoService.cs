using Dapper;
using HttpClientExample.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace HttpClientExample.Services
{
    public class PhotoService(HttpClient client, IConfiguration configuration)
    {
        // using primary constructor
        private readonly HttpClient _client = client;
        private readonly string baseUrl = "https://jsonplaceholder.typicode.com";
        private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection");

        // using primary constructor instead
        //public PhotoService(HttpClient client)
        //{
        //    _client = client;
        //}

        // insert photos into db
        public async Task<List<PhotoDto>> InsertPhotosIntoDb(List<PhotoDto> photos)
        {
            var connection = new SqlConnection(_connectionString);
            string query = @"INSERT INTO Photos (AlbumId, Id, Title, Url, ThumbnailUrl)
                             Values (@AlbumId, @Id, @Title, @Url, @ThumbnailUrl)";
            foreach (var photo in photos)
            {
                await connection.ExecuteAsync(query, photo);
            }
            return photos;
        }

        // get photos from api
        public async Task<List<PhotoDto>> GetAllPhotosFromApi()
        {
            string endpoint = "/photos";
            try
            {
                var photos = await _client.GetFromJsonAsync<List<PhotoDto>>(baseUrl + endpoint);
                return photos ?? [];
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Request Error {ex.Message}");
                return [];
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error Parsing JSON: {ex.Message}");
                return [];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return [];
            }
        }

        // get photos from db
        /*
        public async Task<List<PhotoDto>> GetAllPhotosFromDb()
        {
            using var connection = new SqlConnection(_connectionString);

            string query = @"";
        }
        */

        public async Task<PhotoDto> GetPhotoById(int id)
        {
            string endpoint = $"{baseUrl}/photos/{id}";

            try
            {
                var photo = await _client.GetFromJsonAsync<PhotoDto>(endpoint);
                return photo ?? new PhotoDto
                {
                    AlbumId = 0,
                    Id = 0,
                    Title = string.Empty,
                    Url = string.Empty,
                    ThumbnailUrl = string.Empty
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new PhotoDto
                {
                    AlbumId = 0,
                    Id = 0,
                    Title = string.Empty,
                    Url = string.Empty,
                    ThumbnailUrl = string.Empty
                };
            }
        }
    }
}
