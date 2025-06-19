using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpClientExample.Services
{
    public class CommentService
    {
        readonly HttpClient _client = new();
        string baseUrl = "https://jsonplaceholder.typicode.com";

        public CommentService(HttpClient client)
        {
            _client = client;
        }


    }
}
