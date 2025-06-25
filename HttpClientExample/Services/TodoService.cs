using HttpClientExample.Models;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace HttpClientExample.Services
{
    public class TodoService
    {
        private readonly HttpClient _client;
        private readonly string baseUrl = "https://jsonplaceholder.typicode.com";

        public TodoService(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<TodoDto>> GetAllTodos()
        {
            string endpoint = "/todos";
            try
            {
                var todos = await _client.GetFromJsonAsync<List<TodoDto>>(baseUrl + endpoint);
                return todos ?? new List<TodoDto>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Error: {ex.Message}");
                return new List<TodoDto>();
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"ERROR Parsing JSON: {ex.Message}");
                return new List<TodoDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<TodoDto>();
            }
        }

        public async Task<TodoDto> GetTodoById(int id)
        {
            string endpoint = $"/todos{id}";
            try
            {
                var todo = await _client.GetFromJsonAsync<TodoDto>(baseUrl + endpoint);
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
        public async Task<List<TodoDto>> GetCompletedTodos()
        {
            string endpoint = $"{baseUrl}/todos";
            try
            {
                var todos = await _client.GetFromJsonAsync<List<TodoDto>>(endpoint);
                return (todos ?? new List<TodoDto>()).Where(todo => todo.Completed).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<TodoDto>();
            }
        }

        public async Task<List<TodoDto>> GetIncompleteTodos()
        {
            string endpoint = $"{baseUrl}/todos";
            try
            {
                var incompleteTodos = await _client.GetFromJsonAsync<List<TodoDto>>(endpoint);
                return (incompleteTodos ?? new List<TodoDto>()).Where(todo => !todo.Completed).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<TodoDto>();
            }
        }
    }
}