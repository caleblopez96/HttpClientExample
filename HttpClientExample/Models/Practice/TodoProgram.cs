// Step 1: Get the response from the api (200, 202, 400, 404, etc...)
// Steo 2: Read content from the response (response.Content)
// Step 3: Deserialize the JSON response

using System.Text.Json;
using System.Text.Json.Serialization;

namespace HttpClientExample.Models.Practice
{
    class TodoProgram
    {
        readonly HttpClient client = new();

        //static async Task Main()
        //{
        //    Program program = new();
        //    await program.GetTodoItems();
        //}

        private async Task GetTodoItems()
        {
            // get response
            HttpResponseMessage response = await client.GetAsync("https://jsonplaceholder.typicode.com/todos");

            // read content from response
            string content = await response.Content.ReadAsStringAsync();

            // deserialize the JSON content
            List<Todo>? todos = JsonSerializer.Deserialize<List<Todo>>(content);

            if (todos != null)
            {
                foreach (var todo in todos)
                {
                    Console.WriteLine($"Todo: {todo.Title}: Completed: {todo.Completed}");
                }
            }
            else
            {
                Console.WriteLine("Failed to deserialize the response.");
            }
        }

        class Todo
        {
            [JsonPropertyName("userId")]
            public int UserId { get; set; }

            [JsonPropertyName("id")]
            public int Id { get; set; }

            [JsonPropertyName("title")]
            public string? Title { get; set; }

            [JsonPropertyName("completed")]
            public bool Completed { get; set; }
        }
    }
}
// JSON Payload
/*
 {
  "userId": 1,
  "id": 1,
  "title": "delectus aut autem",
  "completed": false
}

 */