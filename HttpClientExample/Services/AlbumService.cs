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
    public class AlbumService(HttpClient client, IConfiguration configuration)
    {
        private readonly HttpClient _client = client;
        private readonly string baseUrl = "https://jsonplaceholder.typicode.com";
        private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection");

        // using primary constructor instead
        // public AlbumService(HttpClient client, IConfiguration configuration)
        // {
        //    _client = client;
        //    _configuration = configuration
        // }

        // insert albums into the db
        public async Task<List<AlbumDto>> InsertAlbumbsIntoDb(List<AlbumDto> albums)
        {
            using var connection = new SqlConnection(_connectionString);
            string query = @"INSERT INTO Albums (UserId, Id, Title)
                             Values (@UserId, @Id, @Title)";
            foreach (var album in albums)
            {
                await connection.ExecuteAsync(query, album);
            }
            return albums;
        }

        // get albums from api
        public async Task<List<AlbumDto>> GetAllAlbumsFromApi()
        {
            string endpoint = "/albums";
            try
            {
                var albums = await _client.GetFromJsonAsync<List<AlbumDto>>(baseUrl + endpoint);
                return albums ?? [];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return [];
            }
        }

        // get all albums from db
        public async Task<List<AlbumDto>> GetAllAlbumsFromDb()
        {
            using var connection = new SqlConnection(_connectionString);
            string query = @"SELECT UserId, Id, Title FROM Albums;";

            // Correcting the type mismatch by using QueryAsync with IEnumerable<AlbumDto>
            IEnumerable<AlbumDto> dbAlbums = await connection.QueryAsync<AlbumDto>(query);
            return dbAlbums.ToList();
        }

        // helper method to find differences in objects
        private bool AlbumsAreEqual(AlbumDto objectA, AlbumDto objectB)
        {
            return objectA.Title == objectB.Title &&
                   objectA.UserId == objectB.UserId &&
                   objectA.Id == objectB.Id;
        }

        // helper method to update object
        public async Task UpdateAlbumsInDb(List<AlbumDto> albums)
        {
            using var connection = new SqlConnection(_connectionString);
            string query = @"UPDATE Albums
                     SET UserId = @UserId, Title = @Title
                     WHERE Id = @Id";

            foreach (var album in albums)
            {
                await connection.ExecuteAsync(query, album);
            }
        }


        // sync albums with api
        public async Task SyncAlbumsWithApi()
        {
            // Fetch albums from API and database
            List<AlbumDto> apiAlbums = await GetAllAlbumsFromApi();
            List<AlbumDto> dbAlbums = await GetAllAlbumsFromDb();

            // Initialize lists for new and updated albums
            List<AlbumDto> newAlbums = [];
            List<AlbumDto> updatedAlbums = [];

            foreach (var apiAlbum in apiAlbums)
            {
                AlbumDto? dbAlbum = dbAlbums.FirstOrDefault(a => a.Id == apiAlbum.Id);
                if (dbAlbum == null)
                {
                    newAlbums.Add(apiAlbum);
                }
                else if (!AlbumsAreEqual(apiAlbum, dbAlbum))
                {
                    updatedAlbums.Add(apiAlbum);
                }
            }

            if (newAlbums.Count > 0)
            {
                await InsertAlbumbsIntoDb(newAlbums);
                Console.WriteLine($"Inserted {newAlbums.Count} albums");
            }

            if (updatedAlbums.Count > 0)
            {
                await UpdateAlbumsInDb(updatedAlbums);
                Console.WriteLine($"Updated Albums");
            }

            if (newAlbums.Count == 0 && updatedAlbums.Count == 0)
            {
                Console.WriteLine("No changes detected in Albums");
            }
        }

        // api practice, not used for syncing
        /*public async Task<AlbumDto> GetAlbumById(int id)
        {
            string endpoint = $"/albums/{id}";
            try
            {
                AlbumDto? album = await _client.GetFromJsonAsync<AlbumDto>(baseUrl + endpoint);
                return album ?? new AlbumDto();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                return new AlbumDto();
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Json Error: {ex.Message}");
                return new AlbumDto();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new AlbumDto();
            }
        } 
        */
    }
}
