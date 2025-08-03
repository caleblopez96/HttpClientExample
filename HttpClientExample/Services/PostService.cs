using System.Net.Http.Json;
using HttpClientExample.Models;
using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Immutable;

namespace HttpClientExample.Services
{
    public class PostService(HttpClient client, IConfiguration configuration)
    {
        // using primary constructor
        private readonly HttpClient _client = client;
        private readonly string? _connectionString = configuration.GetConnectionString("DefaultConnection");
        private readonly string baseUrl = "https://jsonplaceholder.typicode.com";
        private readonly string postEndPoint = "/posts";

        public async Task<List<PostDto>> GetAllPostFromApi()
        {
            try
            {
                List<PostDto>? posts = await _client.GetFromJsonAsync<List<PostDto>>(baseUrl + postEndPoint);
                return posts ?? [];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting post from API: {ex.Message}");
                return [];
            }
        }


        public async Task<List<PostDto>> GetAllPostFromDb()
        {
            try
            {
                var connection = new SqlConnection(_connectionString);
                string query = @"SELECT 
                                 UserId, Id, Title, Body
                                 FROM Posts";
                IEnumerable<PostDto> dbPosts = await connection.QueryAsync<PostDto>(query);
                return [.. dbPosts];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting all post from db: {ex.Message}");
                return [];
            }
        }

        public async Task<List<PostDto>> InsertPostIntoDb(List<PostDto> posts)
        {
            try
            {
                var connection = new SqlConnection(_connectionString);
                string query = @"INSERT INTO Posts(UserId, Id, Title, Body)
                                 Values (@UserId, @Id, @Title, @Body)";

                foreach (var post in posts)
                {
                    await connection.ExecuteAsync(query, post);
                }
                return posts;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting post into db: {ex.Message}");
                return [];
            }
        }

        public async Task UpdatePostInDb(List<PostDto> posts)
        {
            var connection = new SqlConnection(_connectionString);
            string query = @"UPDATE Posts
                             SET PostId = @PostId, Id = @Id, Title = @Title, Body = @Body
                             WHERE Id = @Id";

            foreach (var post in posts)
            {
                await connection.ExecuteAsync(query, posts);
            }
        }

        public async Task DeletePostFromDb(int id)
        {
            var connection = new SqlConnection(_connectionString);
            string query = @"DELETE FROM Posts
                            WHERE Id = @Id";
            await connection.ExecuteAsync(query, id);
        }

        private bool PostAreEqual(PostDto postA, PostDto postB)
        {
            if (postA == null || postB == null)
                return false;
            bool postEqual = postA.UserId == postB.UserId ||
                             postA.Id == postB.Id ||
                             postA.Title == postB.Title ||
                             postA.Body == postB.Body;
            return postEqual;
        }

        public async Task SyncPostWithApi()
        {
            // get post from api and db
            List<PostDto> apiPosts = await GetAllPostFromApi();
            List<PostDto> dbPosts = await GetAllPostFromDb();


            // create list to hold new post
            List<PostDto> newPosts = new();
            // create list to hold updated post
            List<PostDto> updatedPosts = new();

            // compare each api post with db post
            foreach (var apiPost in apiPosts)
            {
                PostDto? dbPost = dbPosts.FirstOrDefault(p => p.Id == apiPost.Id);
                if (dbPost == null)
                {
                    newPosts.Add(apiPost);
                }
                else if (!PostAreEqual(apiPost, dbPost))
                {
                    updatedPosts.Add(apiPost);
                }
            }

            // find post that are in db that arent in api anymore
            var postsToDelete = dbPosts.Where(db => !apiPosts.Any(api => api.Id == db.Id)).ToList();

            // insert new post
            if (newPosts.Count > 0)
            {
                await InsertPostIntoDb(newPosts);
                Console.WriteLine($"Inserted {newPosts.Count} post into the db");
            }
            // update changedpost
            if (updatedPosts.Count > 0)
            {
                await UpdatePostInDb(updatedPosts);
                Console.WriteLine($"Inserted {updatedPosts.Count} updated post in the db");
            }
            // delete removed post
            if (postsToDelete.Count > 0)
            {
                foreach (var post in postsToDelete)
                {
                    await DeletePostFromDb(post.Id);
                }
                Console.WriteLine($"Deleted {postsToDelete.Count} posts from the db");
            }

            // log if no changes were detected
            if (newPosts.Count == 0 && updatedPosts.Count == 0 && postsToDelete.Count == 0)
            {
                Console.WriteLine($"No changes detected in posts");
            }
        }

        public async Task<PostDto?> GetPostByIdAsync(int postId)
        {
            string endPoint = $"/posts/{postId}";
            try
            {
                var post = await _client.GetFromJsonAsync<PostDto>(baseUrl + endPoint);
                return post;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
