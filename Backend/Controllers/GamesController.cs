using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace Backend.Controllers
{
    /// <summary>
    /// Defines routes and handles actions related to games.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public GamesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Example()
        {
            using (var client = new HttpClient())
            {

                // Send a request to the Twitch authorization API to get an access token
                var authParamString = $"?client_id={_configuration["Twitch:ClientId"] ?? ""}&client_secret={_configuration["Twitch:ClientSecret"] ?? ""}&grant_type=client_credentials";
                var authUri = "https://id.twitch.tv/oauth2/token";
                var authResponse = client.PostAsync(authUri + authParamString, null).Result;
                var authObject = JObject.Parse(authResponse.Content.ReadAsStringAsync().Result);
                var accessToken = authObject["access_token"];

                // Calculate the start and end of the current week in Unix time
                var startOfWeek = new DateTimeOffset(DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek)).ToUnixTimeSeconds();
                var endOfWeek = new DateTimeOffset(DateTime.Today.AddDays(7 - (int)DateTime.Today.DayOfWeek)).ToUnixTimeSeconds();

                // Use the access token to send a request to the IGDB API
                var apiRequest = new HttpRequestMessage();
                apiRequest.Headers.Add("Client-ID", _configuration["Twitch:ClientId"] ?? "");
                apiRequest.Headers.Add("Authorization", "Bearer " + accessToken);
                apiRequest.Content = new StringContent($"fields name, cover.url, hypes, first_release_date;\r\nwhere first_release_date > {startOfWeek} & first_release_date < {endOfWeek} & hypes != null;\r\nsort hypes desc;\r\nlimit 100;");
                apiRequest.Method = HttpMethod.Post;
                apiRequest.RequestUri = new Uri("https://api.igdb.com/v4/games");
                var apiResponse = client.SendAsync(apiRequest).Result;

                var responseObject = JsonSerializer.Deserialize(apiResponse.Content.ReadAsStringAsync().Result, typeof(Object));
                return Ok(responseObject);
            }
        }
    }
}
