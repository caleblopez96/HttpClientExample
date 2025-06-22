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
    public class AlbumService
    {
        private readonly HttpClient _client;
        private readonly string baseUrl = "https://jsonplaceholder.typicode.com";

        public AlbumService(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<AlbumDto>> GetAllAlbumsAsync()
        {
            string endpoint = "/albums";
            try
            {
                var albums = await _client.GetFromJsonAsync<List<AlbumDto>>(baseUrl + endpoint);
                return albums ?? new List<AlbumDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<AlbumDto>();
            }
        }

        public async Task<AlbumDto> GetAlbumById(int id)
        {
            string endpoint = $"/albums/{id}";
            try
            {
                AlbumDto? album = await _client.GetFromJsonAsync<AlbumDto>(baseUrl + endpoint);
                return album ?? new AlbumDto();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                return new AlbumDto();
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Json Error: {ex.Message}");
                return new AlbumDto();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new AlbumDto();
            }
        }
    }
}
