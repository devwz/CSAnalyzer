using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CSAnalyzer
{
    public class FlowToken
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
    }

    public class FlowHttpClient
    {
        public async Task<string> GetFlowTokenAsync()
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("FlowTenant", "");

                var requestData = new
                {
                    clientId = "",
                    clientSecret = "",
                    appToAccess = "llm-api"
                };

                var response = await PostAsync(httpClient, "", requestData);
                return await response.Content.ReadAsStringAsync();
            }
        }

        public async Task PromptAsync(string prompt)
        {
            string flowToken= await GetFlowTokenAsync();

            /*
            if (!IsTokenValid(flowToken))
            {
                await GetFlowTokenAsync();
            }
            */

            var token = JsonSerializer.Deserialize<FlowToken>(flowToken);

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("FlowTenant", "");
                httpClient.DefaultRequestHeaders.Add("FlowAgent", "simple_agent");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

                var requestData = new
                {
                    stream = false,
                    messages = new[]
                    {
                        new
                        {
                            role = "user",
                            content = prompt,
                        }
                    },
                    max_tokens = 3000,
                    model = "gpt-4o-mini"
                };

                var response = await PostAsync(httpClient, "", requestData);
                var result = await response.Content.ReadAsStringAsync();
                
                var flowResult = JsonSerializer.Deserialize<FlowResult>(result);

                using (StreamWriter outputFile = new StreamWriter("Flow ArchReview", false))
                {
                    outputFile.WriteLine(flowResult.Choices[0].Message.Content);
                }
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
