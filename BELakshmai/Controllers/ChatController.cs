using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace BESandbox.Controllers
{
    [ApiController]
    [Route("chat")]
    public class ChatController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public ChatController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] ChatMessageRequest request)
        {
            var chatConfig = _configuration.GetSection("ChatConfig");
            var appName = chatConfig["AppName"];
            var userId = string.IsNullOrEmpty(request.UserId) ? chatConfig["UserId"] : request.UserId;
            var sessionId = string.IsNullOrEmpty(request.SessionId) ? chatConfig["SessionId"] : request.SessionId;
            var streaming = bool.Parse(chatConfig["Streaming"] ?? "false");

            var url = "http://localhost:8000/run";
            var body = new
            {
                appName = appName,
                userId = userId,
                sessionId = sessionId,
                newMessage = new
                {
                    parts = new[] { new { text = request.Text } },
                    role = "user"
                },
                streaming = streaming,
                stateDelta = new { additionalProp1 = new { } }
            };
            var jsonBody = JsonConvert.SerializeObject(body);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            using (var httpClient = new HttpClient())
            {
                var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
                httpRequest.Headers.Add("accept", "application/json");
                httpRequest.Content = content;

                var response = await httpClient.SendAsync(httpRequest);
                var responseContent = await response.Content.ReadAsStringAsync();
                try
                {
                    var token = JToken.Parse(responseContent);
                    if (token is JArray arr)
                    {
                        // Try to extract the text from the first element
                        var first = arr.FirstOrDefault();
                        var text = first?["content"]?["parts"]?.FirstOrDefault()?["text"]?.ToString();
                        if (!string.IsNullOrEmpty(text))
                        {
                            return Ok(new { text });
                        }
                        return Ok(new { error = "Text not found in response", response = responseContent });
                    }
                    else if (token is JObject obj)
                    {
                        // Fallback: try to extract text if the response is an object
                        var text = obj["content"]?["parts"]?.FirstOrDefault()?["text"]?.ToString();
                        if (!string.IsNullOrEmpty(text))
                        {
                            return Ok(new { text });
                        }
                        return Ok(new { error = "Text not found in response", response = responseContent });
                    }
                    else
                    {
                        return Ok(new { error = "Unknown JSON structure", response = responseContent });
                    }
                }
                catch (JsonReaderException)
                {
                    // Not valid JSON, return the raw response
                    return Ok(new { error = "Invalid JSON response from external API", response = responseContent });
                }
            }
        }
    }

    public class ChatMessageRequest
    {
        public string Text { get; set; }
        public string UserId { get; set; }
        public string SessionId { get; set; }
    }
} 