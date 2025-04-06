using GDMS_API.Interfaces;
using System.Text.Json;
using System.Text;
using static System.Collections.Specialized.BitVector32;
using Newtonsoft.Json.Linq;
using GDMS_API.Models;
using System.Net.Http.Headers;
using System.Collections.Generic;
using logger;


namespace GDMS_API.Services
{
    public class ApiService : IService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public object JsonConvert { get; private set; }

        public async Task<string> GetToken()
        {
            try
            {

                Serilog_Logger.LogInformation("Start GenerateToken");
                var accessTokensection = _configuration.GetSection("getAccessToken");
                var json = JsonSerializer.Serialize(accessTokensection.GetChildren().ToDictionary(x => x.Key, x => x.Value));
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(_configuration["URLForGenerateToken"]),
                    Content = content
                };

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var jsonObject = JsonSerializer.Deserialize<TokenResponce>(responseContent);
                    return jsonObject.access_token;
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                    Serilog_Logger.LogError(response.StatusCode + "Function : GetToken ");
                    return null;
                }
            }
            catch (Exception ex)
            {

                Serilog_Logger.LogError(ex.Message.ToString() + "Function : GetToken ");
                return null;
            }
        }
        public async Task<OwnerInfos> GetTitleByRen(string REN, string Token)
        {
            try
            {
                Serilog_Logger.LogInformation("Start GetOwnerInfo");
                var accessTokensection = _configuration.GetSection("landRegistry");
                var json = JsonSerializer.Serialize(accessTokensection.GetChildren().ToDictionary(x => x.Key, x => x.Value));
                var content = new StringContent(REN, Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(_configuration["URLForlandRegistry"] + REN),
                };
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    OwnerInfos ownerInfos = JsonSerializer.Deserialize<OwnerInfos>(responseContent);
                    return ownerInfos;
                }
                else
                {
                 
                    Serilog_Logger.LogError("Response Requset Stutas :  " +response.StatusCode + " Function : GetTitleByRen \n Please check API URL: " + _configuration["URLForlandRegistry"] + "it is not working correctly" );
                    return null;
                }
            }
            catch (Exception ex)
            {

                Serilog_Logger.LogError(ex.Message.ToString() + "Function : GetTitleByRen ");
                return null;
            }

        }
    }
}
