//JSON-to-C# Object Mapping

using System.Text.Json.Serialization;


namespace HttpClientExample.Models.Practice
{
    internal class PeopleResponse
    {
        public class ApiResponse
        {
            public bool Success { get; set; }
            [JsonPropertyName("people")]
            public List<Person>? People { get; set; }
        }

        public class Person
        {
            [JsonPropertyName("id")]
            public required int Id { get; set; }
            [JsonPropertyName("full_name")]
            public required FullName FullName { get; set; }

            [JsonPropertyName("email")]
            public required string EmailAddress { get; set; }

            [JsonPropertyName("gender")]
            public required string Gender { get; set; }

            [JsonPropertyName("employment")]
            public Employment? EmploymentInfo { get; set; }

            [JsonPropertyName("contacts")]
            public List<Contact>? Contacts { get; set; }

        }

        public class FullName
        {
            [JsonPropertyName("first")]
            public string? FirstName { get; set; }

            [JsonPropertyName("last")]
            public string? LastName { get; set; }


        }

        public class Employment
        {
            [JsonPropertyName("job_title")]
            public string? JobTitle { get; set; }

            [JsonPropertyName("company")]
            public Company? CompanyInfo { get; set; }

            [JsonPropertyName("start_date")]
            public string? StartDate { get; set; }

        }

        public class Company
        {
            [JsonPropertyName("name")]
            public string? CompanyName { get; set; }

            [JsonPropertyName("location")]
            public string? Location { get; set; }
        }

        public class Contact
        {
            [JsonPropertyName("type")]
            public required string Type { get; set; }

            [JsonPropertyName("number")]
            public string? Number { get; set; }

            [JsonPropertyName("address")]
            public string? Address { get; set; }
        }
    }
}
// JSON PAYLOAD
/*
 {
  "success": true,
  "people": [
    {
      "id": 1,
      "full_name": {
        "first": "Benedict",
        "last": "Trussell"
      },
      "email": "btrussell0@paypal.com",
      "gender": "Male",
      "employment": {
        "job_title": "Software Developer",
        "company": {
          "name": "TechCorp",
          "location": "New York"
        },
        "start_date": "2022-01-15"
      },
      "contacts": [
        {
          "type": "mobile",
          "number": "555-1234"
        },
        {
          "type": "email",
          "address": "benedict.work@techcorp.com"
        }
      ]
    },
    {
      "id": 2,
      "full_name": {
        "first": "Quillan",
        "last": "De Ruel"
      },
      "email": "qderuel1@odnoklassniki.ru",
      "gender": "Male",
      "employment": null,
      "contacts": []
    }
  ]
}
 */