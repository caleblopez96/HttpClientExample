using HttpClientExample.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace HttpClientExample.Services
{
    public class PhotoService
    {
        private readonly HttpClient _client;
        private readonly string baseUrl = "https://jsonplaceholder.typicode.com";

        public PhotoService(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<PhotoDto>> GetAllPhotosAsync()
        {
            string endpoint = "/photos";
            try
            {
                var photos = await _client.GetFromJsonAsync<List<PhotoDto>>(baseUrl + endpoint);
                return photos ?? new List<PhotoDto>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Request Error {ex.Message}");
                return new List<PhotoDto>();
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error Parsing JSON: {ex.Message}");
                return new List<PhotoDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<PhotoDto>();
            }
        }

        public async Task<PhotoDto> GetPhotoById(int id)
        {
            string endpoint = $"{baseUrl}/photos/{id}";

            try
            {
                var photo = await _client.GetFromJsonAsync<PhotoDto>(endpoint);
                return photo ?? new PhotoDto();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new PhotoDto();
            }
        }
    }
}
