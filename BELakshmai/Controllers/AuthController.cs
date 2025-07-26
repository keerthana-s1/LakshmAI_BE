using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace BELakshmai.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // Use the password as the token for the external call
            var token = request.SessionId;

            // Prepare the URL
            var url = $"http://localhost:8000/apps/lakshmai-agent/users/{request.UserId}/sessions/{token}";

            // Prepare the body
            var body = new { additionalProp1 = new { } };
            var jsonBody = JsonConvert.SerializeObject(body);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            using (var httpClient = new HttpClient())
            {
                var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
                httpRequest.Headers.Add("accept", "application/json");
                httpRequest.Content = content;

                var response = await httpClient.SendAsync(httpRequest);
                var responseContent = await response.Content.ReadAsStringAsync();
                // Log the response content for debugging
                System.Console.WriteLine($"External API response: {responseContent}");
                // Parse the response and return all details
                var json = JObject.Parse(responseContent);
                var sessionid = json["sessionid"] ?? json["id"] ?? json["sessionId"];
                var result = json.ToObject<Dictionary<string, object>>();
                if (sessionid != null)
                {
                    result["sessionid"] = sessionid;
                    return Ok(result);
                }
                result["error"] = "sessionid not found in response";
                return Ok(result);
            }
        }
    }

    public class LoginRequest
    {
        public string UserId { get; set; }
        public string Password { get; set; }

        public string SessionId { get; set; }
    }
}
