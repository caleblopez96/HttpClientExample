using HttpClientExample.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using Dapper;

namespace HttpClientExample.Services
{
    public class TodoService(HttpClient client, IConfiguration configuration)
    {
        // using primary constructor
        private readonly HttpClient _client = client;
        private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection");
        private readonly string _baseUrl = "https://jsonplaceholder.typicode.com";
        private readonly string _todosEndPoint = "/todos";

        public enum TodoStatus
        {
            Completed,
            Incomplete
        }

        // get todos from api
        public async Task<List<TodoDto>> GetTodosFromApi()
        {
            try
            {
                List<TodoDto>? todos = await _client.GetFromJsonAsync<List<TodoDto>>(_baseUrl + _todosEndPoint);
                return todos ?? [];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception($"Error fetching todos from API {ex.Message}");
            }
        }

        // get todos from db
        public async Task<List<TodoDto>> GetTodosFromDb()
        {
            try
            {
                await using SqlConnection connection = new SqlConnection(_connectionString);
                string query = @"SELECT Id, UserId, Title, Completed 
                                 FROM Todos";
                IEnumerable<TodoDto> dbTodos = await connection.QueryAsync<TodoDto>(query);
                return [.. dbTodos];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting all posts from db: {ex.Message}");
                return [];
            }
        }

        // insert todos into db
        public async Task<List<TodoDto>> InsertTodosIntoDb(List<TodoDto> todos)
        {
            try
            {
                var connection = new SqlConnection(_connectionString);
                string query = @"INSERT INTO Todos (Id, UserId, Title, Completed)
                                 VALUES (@Id, @UserId, @Title, @Completed)";
                foreach (var todo in todos)
                {
                    await connection.ExecuteAsync(query, todo);
                }
                return todos;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting todos into db {ex.Message}");
                return [];
            }

        }

        // update todos in db
        public async Task UpdateTodosInDb(List<TodoDto> updatedTodos)
        {
            using var connection = new SqlConnection(_connectionString);
            string query = @"UPDATE Todos
                             SET UserId = @UserId, Id = @Id, Title = @Title, Completed = @Completed 
                             WHERE Id = @Id";
            foreach (var todo in updatedTodos)
            {
                await connection.ExecuteAsync(query, todo);
            }
        }

        // delete todo in db
        public async Task DeleteTodosFromDb(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            string query = @"DELETE FROM Todos
                             WHERE Id = @Id";
            await connection.ExecuteAsync(query, id);
        }

        // check if todos are equal
        public bool TodosAreEqual(TodoDto todoA, TodoDto todoB)
        {
            if (todoA == null || todoB == null)
                return false;
            bool todosAreEqual = todoA.Id == todoB.Id || todoA.UserId == todoB.UserId || todoA.Title == todoB.Title || todoA.Completed == todoB.Completed;
            return todosAreEqual;
        }

        // sync todos with api
        public async Task SyncTodosWithApi()
        {
            // get post from api and db
            List<TodoDto> dbTodos = await GetTodosFromApi();
            List<TodoDto> apiTodos = await GetTodosFromApi();

            // create list to hold newtodos
            List<TodoDto> newTodos = new();
            List<TodoDto> updatedTodos = new();

            // compare each api post with db post
            foreach (var apiTodo in apiTodos)
            {
                TodoDto? dbTodo = dbTodos.FirstOrDefault(t => t.Id == apiTodo.Id);
                if (dbTodo == null)
                {
                    newTodos.Add(apiTodo);
                }
                else if (!TodosAreEqual(apiTodo, dbTodo))
                {
                    updatedTodos.Add(apiTodo);
                }
            }

            // find todos that are in db but not in api
            var todosToDelete = dbTodos.Where(db => !apiTodos.Any(api => api.Id == db.Id)).ToList();

            // insert new todo
            if (newTodos.Count > 0)
            {
                await InsertTodosIntoDb(newTodos);
                Console.WriteLine($"Inserted {newTodos.Count} todos into the db");
            }

            // update changed todos
            if (updatedTodos.Count > 0)
            {
                await UpdateTodosInDb(updatedTodos);
                Console.WriteLine($"Inserted {updatedTodos.Count} updated todos into the db");
            }

            // delete removed todos
            if (todosToDelete.Count > 0)
            {
                foreach (var todo in todosToDelete)
                {
                    await DeleteTodosFromDb(todo.Id);
                }
                Console.WriteLine($"Deleted {todosToDelete.Count} todos from the db");
            }

            // log no changes detected
            if (newTodos.Count == 0 && updatedTodos.Count == 0 && todosToDelete.Count == 0)
            {
                Console.WriteLine("No changes detected in todos");
            }
        }

        public async Task<TodoDto> GetTodoById(int id)
        {
            string endpoint = $"{_baseUrl}/todos{id}";
            try
            {
                var todo = await _client.GetFromJsonAsync<TodoDto>(endpoint);
                return todo ?? new TodoDto
                {
                    UserId = 0,
                    Id = 0,
                    Title = string.Empty,
                    Completed = false,
                };
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Error: {ex.Message}");
                return new TodoDto
                {
                    UserId = 0,
                    Id = 0,
                    Title = string.Empty,
                    Completed = false,
                };
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"ERROR Parsing JSON: {ex.Message}");
                return new TodoDto
                {
                    UserId = 0,
                    Id = 0,
                    Title = string.Empty,
                    Completed = false,
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new TodoDto
                {
                    UserId = 0,
                    Id = 0,
                    Title = string.Empty,
                    Completed = false,
                };
            }
        }

        public async Task<List<TodoDto>> GetTodosBasedOnStatus(TodoStatus Completed)
        {
            string endpoint = $"{_baseUrl}/todos";
            try
            {
                var todosBasedOnStatus = await _client.GetFromJsonAsync<List<TodoDto>>(endpoint);
                if (todosBasedOnStatus == null)
                {
                    return [];
                }
                bool isComplete = Completed == TodoStatus.Completed;
                return [.. todosBasedOnStatus.Where(x => x.Completed == isComplete)];
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Request Exception: {ex.Message}");
                return [];
            }
        }
        // i combined the two methods below into one method GetTodosBasedOnStatus, but don't know if its best practice
        // leaving these here for now

        //public async Task<List<TodoDto>> GetCompletedTodos()
        //{
        //    string endpoint = $"{baseUrl}/todos";
        //    try
        //    {
        //        var todos = await _client.GetFromJsonAsync<List<TodoDto>>(endpoint);
        //        return [.. (todos ?? []).Where(todo => todo.Completed)];
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error: {ex.Message}");
        //        return [];
        //    }
        //}

        //public async Task<List<TodoDto>> GetIncompleteTodos()
        //{
        //    string endpoint = $"{baseUrl}/todos";
        //    try
        //    {
        //        var incompleteTodos = await _client.GetFromJsonAsync<List<TodoDto>>(endpoint);
        //        return [.. (incompleteTodos ?? []).Where(todo => !todo.Completed)];
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error: {ex.Message}");
        //        return [];
        //    }
        //}
    }
}