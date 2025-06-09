using System.Text.Json.Serialization;


namespace HttpClientExample.Models.Practice
{
    internal class ProductResponse
    {
        public class ApiResponse
        {
            [JsonPropertyName("success")]
            public required bool Success { get; set; }

            public required List<Product> Products { get; set; }
        }

        public class Product
        {
            [JsonPropertyName("product_id")]
            public required int ProductId { get; set; }

            [JsonPropertyName("name")]
            public string? Name { get; set; }

            [JsonPropertyName("price")]
            public double? Price { get; set; }

            [JsonPropertyName("in_stock")]
            public bool? InStock { get; set; }

            [JsonPropertyName("category")]
            public Category? Category { get; set; }

            [JsonPropertyName("specifications")]
            public Specification? Specification { get; set; }
        }

        public class Category
        {
            [JsonPropertyName("category_id")]
            public required int CategoryId { get; set; }

            [JsonPropertyName("category_name")]
            public string? CategoryName { get; set; }
        }

        public class Specification
        {
            [JsonPropertyName("color")]
            public string? Color { get; set; }

            [JsonPropertyName("connection")]
            public string? Connection { get; set; }

            [JsonPropertyName("battery_life")]
            public string? BatteryLife { get; set; }

            [JsonPropertyName("switch_type")]
            public string? SwitchType { get; set; }

            [JsonPropertyName("backlight")]
            public string? Backlight { get; set; }
        }
    }
}
