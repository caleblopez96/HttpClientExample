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
        public async Task<List<PhotoDto>> GetAllPhotosFromDb()
        {
            using var connection = new SqlConnection(_connectionString);

            string query = @"SELECT AlbumId, Id, Title, Url, ThumbnailUrl FROM Photos;";

            IEnumerable<PhotoDto> dbPhotos = await connection.QueryAsync<PhotoDto>(query);
            return [.. dbPhotos];
        }

        // update photos in db
        public async Task UpdatePhotosInDb(List<PhotoDto> photos)
        {
            using var connection = new SqlConnection(_connectionString);
            string query = @"UPDATE Photos
                            SET Title = @Title, Url = @Url, ThumbnailUrl = @ThumbnailUrl
                            WHERE Id = @Id";
            foreach (var photo in photos)
            {
                await connection.ExecuteAsync(query, photo);
            }
        }

        // delete photos from db
        public async Task DeletePhotosFromDb(int id)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            string query = @"DELETE FROM Photos
                             WHERE Id = @Id";
            await connection.ExecuteAsync(query, id);
        }

        // helper method 
        public static bool PhotosAreEqual(PhotoDto photoA, PhotoDto photoB)
        {
            return photoA.Title == photoB.Title &&
                   photoA.Url == photoB.Url &&
                   photoA.ThumbnailUrl == photoB.ThumbnailUrl &&
                   photoA.Id == photoB.Id;
        }

        public async Task SyncPhotosWithApi()
        {
            // get photos from api and db
            List<PhotoDto> apiPhotos = await GetAllPhotosFromApi();
            List<PhotoDto> dbPhotos = await GetAllPhotosFromDb();

            // list to hold new and updated albums
            List<PhotoDto> newPhotos = new List<PhotoDto>();
            List<PhotoDto> updatedPhotos = new List<PhotoDto>();

            foreach (var apiPhoto in apiPhotos)
            {
                PhotoDto? dbPhoto = dbPhotos.FirstOrDefault(p => p.Id == apiPhoto.Id);
                if (dbPhoto == null)
                {
                    newPhotos.Add(apiPhoto);
                }
                else if (!PhotosAreEqual(apiPhoto, dbPhoto))
                {
                    updatedPhotos.Add(apiPhoto);
                }
            }

            var photosToDelete = dbPhotos.Where(db => !apiPhotos.Any(api => api.Id == db.Id)).ToList();

            if (newPhotos.Count > 0)
            {
                await InsertPhotosIntoDb(newPhotos);
                Console.WriteLine($"Inserted {newPhotos.Count} {(newPhotos.Count == 1 ? "Photo" : "Photos")} into the db");
            }

            if (updatedPhotos.Count > 0)
            {
                await UpdatePhotosInDb(updatedPhotos);
                Console.WriteLine($"Updated Photos");
            }

            if (photosToDelete.Count > 0)
            {
                foreach (var photo in photosToDelete)
                {
                    await DeletePhotosFromDb(photo.Id);
                }
                Console.WriteLine($"Deleted {photosToDelete.Count}");
            }

            if (newPhotos.Count == 0 && updatedPhotos.Count == 0)
            {
                Console.WriteLine("No changes detected in photos");
            }
        }

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
