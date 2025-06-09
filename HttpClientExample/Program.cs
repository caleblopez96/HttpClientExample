using System.Text.Json;
using static HttpClientExample.Models.UserResponse; // class containing API response models


namespace HttpClientExample
{
    internal class Program
    {
        readonly HttpClient client = new();

        static async Task Main()
        {
            Program program = new();
            await program.GetUsersData();
        }

        private async Task GetUsersData()
        {
            // Step 1: Get response
            HttpResponseMessage response = await client.GetAsync("https://jsonplaceholder.typicode.com/users");

            // Step 2: Read content from response
            string content = await response.Content.ReadAsStringAsync();

            // Step 3: Deserialize the JSON content
            List<User>? users = JsonSerializer.Deserialize<List<User>>(content);

            if (users != null)
            {
                Console.WriteLine("=== JSONPlaceholder Users ===");
                foreach (var user in users)
                {
                    Console.WriteLine($"User ID: {user.Id}");
                    Console.WriteLine($"Name: {user.Name}");
                    Console.WriteLine($"Username: {user.Username}");
                    Console.WriteLine($"Email: {user.Email}");
                    Console.WriteLine($"Phone: {user.Phone}");
                    Console.WriteLine($"Website: {user.Website}");

                    if (user.Address != null)
                    {
                        Console.WriteLine($"Address: {user.Address.Street}, {user.Address.Suite}");
                        Console.WriteLine($"City: {user.Address.City}, {user.Address.Zipcode}");
                    }

                    if (user.Company != null)
                    {
                        Console.WriteLine($"Company: {user.Company.Name}");
                        Console.WriteLine($"Catchphrase: {user.Company.CatchPhrase}");
                    }

                    Console.WriteLine("---");
                }
            }
            else
            {
                Console.WriteLine("Failed to deserialize the response.");
            }
        }
    }
}
