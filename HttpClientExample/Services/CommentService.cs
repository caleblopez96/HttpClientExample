using Dapper;
using HttpClientExample.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace HttpClientExample.Services
{
    public class CommentService(HttpClient client, IConfiguration configuration)
    {
        // using primary constructor
        readonly HttpClient _client = client;
        readonly string _baseUrl = "https://jsonplaceholder.typicode.com";
        private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection");

        // using primary constructor instead
        //public CommentService(HttpClient client)
        //{
        //    _client = client;
        //}

        public async Task<List<CommentDto>> GetAllComments()
        {
            string _endpoint = "/comments";
            try
            {
                List<CommentDto>? comments = await _client.GetFromJsonAsync<List<CommentDto>>(_baseUrl + _endpoint);
                //return comments ?? new List<CommentDto>();
                return comments ?? [];
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Error: {ex.Message}");
                //return new List<CommentDto>();
                return [];
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Json Error: {ex.Message}");
                //return new List<CommentDto>();
                return [];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                //return new List<CommentDto>();
                return [];
            }
        }

        public async Task<CommentDto> GetCommentByCommentId(int commentId)
        {
            string endpoint = $"/comments/{commentId}";
            try
            {
                CommentDto? comment = await _client.GetFromJsonAsync<CommentDto>(_baseUrl + endpoint);
                return comment ?? new CommentDto();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Error: {ex.Message}");
                return new CommentDto();
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON Error: {ex.Message}");
                return new CommentDto();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new CommentDto();
            }
        }

        // out of workflow method, but this is created
        // to populate the db with comments because nothing is there
        public async Task InsertCommentsIntoDb(List<CommentDto> comments)
        {
            using var connection = new SqlConnection(_connectionString);

            const string insertQuery = @"INSERT INTO Comments(Id, PostId, Name, Email, Body)
                                         VALUES (@Id, @PostId, @Name, @Email, @Body);";


            foreach (var comment in comments)
            {
                await connection.ExecuteAsync(insertQuery, comment);
            }
            Console.WriteLine($"Inserted {comments.Count} comments");
        }
    }
}
