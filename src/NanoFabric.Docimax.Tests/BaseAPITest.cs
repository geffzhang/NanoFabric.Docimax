using System;
using System.Collections.Generic;
using System.Text;
using IdentityModel.Client;
using System.Net.Http;

namespace NanoFabric.Docimax.Tests
{

    public class BaseAPITest
    {
        protected readonly HttpClient _client;

        public BaseAPITest()
        {
            var clientId = "597c2429f4b27d74";
            _client = new HttpClient();
            _client.BaseAddress = new Uri("http://127.0.0.1:8000");

            var tokenClient = new TokenClient($"http://127.0.0.1:8000/connect/token", clientId, "96eb098a127f74bad17badc5ea395b69");
            var tokenResponse = tokenClient.RequestClientCredentialsAsync("lmcore").Result;
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);
            _client.DefaultRequestHeaders.Add("client_id", clientId);
          
        }

    }
}
