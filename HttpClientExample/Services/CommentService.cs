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

        // get comments from api
        public async Task<List<CommentDto>> GetAllCommentsFromApi()
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

        // get comments from db
        public async Task<List<CommentDto>> GetAllCommentsFromDb()
        {
            using var connection = new SqlConnection(_connectionString);
            string query = @"SELECT Id, PostId, Name, Email, Body
                             FROM Comments";
            IEnumerable<CommentDto> dbComments = await connection.QueryAsync<CommentDto>(query);
            return dbComments.ToList();
        }

        // insert comments
        public async Task InsertComments(List<CommentDto> comments)
        {
            using var connection = new SqlConnection(_connectionString);
            string query = @"INSERT INTO Comments(Id, PostId, Name, Email, Body)
                             VALUES (@Id, @PostId, @Name, @Email, @Body)";
            foreach (var comment in comments)
            {
                await connection.ExecuteAsync(query, comment);

            }
        }

        // update comments
        public async Task UpdateComments(List<CommentDto> comments)
        {
            using var connection = new SqlConnection(_connectionString);
            string query = @"UPDATE Comments 
                             SET PostId=@PostId, 
                             Name=@Name, 
                             Email=@Email, 
                             Body=@Body
                             WHERE Id=@Id";
            foreach (var comment in comments)
            {
                await connection.ExecuteAsync(query, comment);
            }
        }

        // delete comment in db
        public async Task DeleteComment(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            string query = "DELETE FROM Comments WHERE Id = @Id";
            await connection.ExecuteAsync(query, new { Id = id });
        }

        // check if objects are equal
        private bool CommentsAreEqual(CommentDto objectA, CommentDto objectB)
        {
            return objectA.PostId == objectB.PostId &&
                   objectA.Name == objectB.Name &&
                   objectA.Email == objectB.Email &&
                   objectA.Body == objectB.Body;
        }

        // sync comments
        public async Task SyncCommentsWithApi()
        {
            // get comments from the API and DB
            List<CommentDto> apiComments = await GetAllCommentsFromApi();
            List<CommentDto> dbComments = await GetAllCommentsFromDb();

            var newComments = new List<CommentDto>();
            var updatedComments = new List<CommentDto>();

            // compare each API comment to DB comment
            foreach (var apiComment in apiComments)
            {
                var dbComment = dbComments.FirstOrDefault(c => c.Id == apiComment.Id);
                if (dbComment == null)
                {
                    newComments.Add(apiComment);
                }
                else if (!CommentsAreEqual(apiComment, dbComment))
                {
                    updatedComments.Add(apiComment);
                }
            }

            // find comments in DB that aren't in the API anymore (deletions)
            var commentsToDelete = dbComments
                .Where(db => !apiComments.Any(api => api.Id == db.Id))
                .ToList();

            // insert new comments
            if (newComments.Count > 0)
            {
                await InsertComments(newComments);
                Console.WriteLine($"Inserted {newComments.Count} comments");
            }

            // update changed comments
            if (updatedComments.Count > 0)
            {
                await UpdateComments(updatedComments);
                Console.WriteLine($"Updated {updatedComments.Count} comments");
            }

            // delete removed comments
            if (commentsToDelete.Count > 0)
            {
                foreach (var comment in commentsToDelete)
                {
                    await DeleteComment((int)comment.Id);
                }
                Console.WriteLine($"Deleted {commentsToDelete.Count} comments");
            }

            // log if no changes were detected
            if (newComments.Count == 0 && updatedComments.Count == 0 && commentsToDelete.Count == 0)
            {
                Console.WriteLine("No changes detected in Comments");
            }
        }


        // api call practice. not a method used for syncing
        /*public async Task<CommentDto> GetCommentByCommentId(int commentId)
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
        */
    }
}

