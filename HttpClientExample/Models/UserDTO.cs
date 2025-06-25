using System.Text.Json.Serialization;

namespace HttpClientExample.Models
{
    public class UserDto
    {
        [JsonPropertyName("id")]
        public required int Id { get; set; }

        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("username")]
        public required string Username { get; set; }

        [JsonPropertyName("email")]
        public required string Email { get; set; }

        [JsonPropertyName("phone")]
        public required string Phone { get; set; }

        [JsonPropertyName("website")]
        public string? Website { get; set; }

        [JsonPropertyName("address")]
        public AddressDTO? Address { get; set; }

        [JsonPropertyName("company")]
        public CompanyDTO? Company { get; set; }
    }

    public class AddressDTO
    {
        [JsonPropertyName("street")]
        public required string Street { get; set; }

        [JsonPropertyName("suite")]
        public string? Suite { get; set; } 

        [JsonPropertyName("city")]
        public required string City { get; set; }

        [JsonPropertyName("zipcode")]
        public required string Zipcode { get; set; }

        [JsonPropertyName("geo")]
        public required GeoDTO Geo { get; set; }
    }

    public class GeoDTO
    {
        [JsonPropertyName("lat")]
        public string Lat { get; set; } = string.Empty;

        [JsonPropertyName("lng")]
        public string Lng { get; set; } = string.Empty;
    }

    public class CompanyDTO
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("catchPhrase")]
        public string CatchPhrase { get; set; } = string.Empty;

        [JsonPropertyName("bs")]
        public string Bs { get; set; } = string.Empty;
    }
}