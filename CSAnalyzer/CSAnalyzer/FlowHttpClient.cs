using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CSAnalyzer
{
    public class FlowHttpClient
    {
        string flowToken;

        public async Task GetFlowTokenAsync()
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("FlowTenant", "");

                var requestData = new
                {
                    clientId = "",
                    clientSecret = "",
                    appToAccess = ""
                };

                var response = await PostAsync(httpClient, "", requestData);
                flowToken = await response.Content.ReadAsStringAsync();
            }
        }

        public async Task PromptAsync()
        {
            if (!IsTokenValid(flowToken))
            {
                await GetFlowTokenAsync();
            }

            using (var httpClient = new HttpClient())
            {
                string token = "";

                httpClient.DefaultRequestHeaders.Add("FlowTenant", "");
                httpClient.DefaultRequestHeaders.Add("FlowAgent", "simple_agent");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var requestData = new
                {
                    stream = false,
                    messages = new[]
                    {
                        new
                        {
                            role = "user",
                            content = "Hello, world!",
                        }
                    },
                    max_tokens = 3000,
                    model = "gpt-4o-mini"
                };

                var response = await PostAsync(httpClient, "", requestData);
                var result = await response.Content.ReadAsStringAsync();
            }
        }

        private bool IsTokenValid(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = (JwtSecurityToken)handler.ReadToken(token);

            return jwtToken != null && jwtToken.ValidTo > DateTime.UtcNow;
        }

        private async Task<HttpResponseMessage> PostAsync(HttpClient httpClient, string url, object data)
        {
            var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            return response;
        }
    }
}
