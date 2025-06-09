using System.Text.Json.Serialization;

namespace HttpClientExample
{
    internal class OrderResponse
    {
        public class ApiResponse
        {
            [JsonPropertyName("success")]
            public required bool Success { get; set; }

            [JsonPropertyName("data")]
            public required List<Order> Orders { get; set; }
        }

        public class Order
        {
            [JsonPropertyName("order_id")]
            public required int OrderId { get; set; }

            [JsonPropertyName("customer")]
            public required Customer Customer { get; set; }

            [JsonPropertyName("items")]
            public List<Item>? Items { get; set; }

            [JsonPropertyName("shipping")]
            public Shipping? ShippingInfo { get; set; }

            [JsonPropertyName("order_status")]
            public required string OrderStatus { get; set; }
        }

        public class Customer
        {
            [JsonPropertyName("customer_id")]
            public required int CustomerId { get; set; }

            [JsonPropertyName("name")]
            public required Name Name { get; set; }

            [JsonPropertyName("email")]
            public required string Email { get; set; }
        }

        public class Name
        {
            [JsonPropertyName("first")]
            public required string FirstName { get; set; }

            [JsonPropertyName("last")]
            public required string LastName { get; set; }
        }

        public class Item
        {
            [JsonPropertyName("product_id")]
            public required int ProductId { get; set; }

            [JsonPropertyName("product_name")]
            public required string ProductName { get; set; }

            [JsonPropertyName("quantity")]
            public required int Quantity { get; set; }

            [JsonPropertyName("price")]
            public required decimal Price { get; set; }
        }

        public class Shipping
        {
            [JsonPropertyName("address")]
            public required string Address { get; set; }

            [JsonPropertyName("method")]
            public required string Method { get; set; }

            [JsonPropertyName("shipped_date")]
            public required string ShippedDate { get; set; }
        }
    }
}