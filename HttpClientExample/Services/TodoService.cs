using HttpClientExample.Models;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace HttpClientExample.Services
{
    public class TodoService(HttpClient client)
    {
        private readonly HttpClient _client = client;
        private readonly string baseUrl = "https://jsonplaceholder.typicode.com";

        public enum TodoStatus
        {
            Complete,
            Incomplete
        }

        public async Task<List<TodoDto>> GetAllTodos()
        {
            string endpoint = $"{baseUrl}/todos";
            try
            {
                var todos = await _client.GetFromJsonAsync<List<TodoDto>>(endpoint);
                return todos ?? [];
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Error: {ex.Message}");
                return [];
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"ERROR Parsing JSON: {ex.Message}");
                return [];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return [];
            }
        }

        public async Task<TodoDto> GetTodoById(int id)
        {
            string endpoint = $"{baseUrl}/todos{id}";
            try
            {
                var todo = await _client.GetFromJsonAsync<TodoDto>(endpoint);
                return todo ?? new TodoDto
                {
                    UserId = 0,
                    Id = 0,
                    Title = string.Empty,
                    Status = false,
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
                    Status = false,
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
                    Status = false,
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
                    Status = false,
                };
            }
        }

        public async Task<List<TodoDto>> GetTodosBasedOnStatus(TodoStatus status)
        {
            string endpoint = $"{baseUrl}/todos";
            try
            {
                var todosBasedOnStatus = await _client.GetFromJsonAsync<List<TodoDto>>(endpoint);
                if (todosBasedOnStatus == null)
                {
                    return [];
                }
                bool isComplete = status == TodoStatus.Complete;
                return [.. todosBasedOnStatus.Where(x => x.Status == isComplete)];
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
        //        return [.. (todos ?? []).Where(todo => todo.Status)];
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
        //        return [.. (incompleteTodos ?? []).Where(todo => !todo.Status)];
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error: {ex.Message}");
        //        return [];
        //    }
        //}
    }
}