using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System;
using BELakshmai.Data;

namespace BELakshmai.Controllers
{
    [ApiController]
    [Route("chat")]
    public class ChatController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        
        // Static mapping of userId to phone number
        private static readonly Dictionary<string, string> UserPhoneMapping = new Dictionary<string, string>
        {
            { "user", "3333333333" },
            { "user1", "1111111111" },
            { "user2", "2222222222" },
            { "testuser", "9999999999" }
        };
        
        // Cache for dashboard responses
        private static readonly Dictionary<string, CachedDashboardResponse> DashboardCache = new Dictionary<string, CachedDashboardResponse>();
        
        // Cache expiration time (5 minutes)
        private static readonly TimeSpan CacheExpiration = TimeSpan.FromMinutes(5);

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

        [HttpPost("LoadHomeDashboard")]
        public async Task<IActionResult> LoadHomeDashboard([FromBody] LoadHomeDashboardRequest request)
        {
            try
            {
                // Check cached responses first
                if (CacheData.CachedDashboardResponses.TryGetValue(request.UserId, out string cachedResponse))
                {
                    try
                    {
                        var parsedResponse = JObject.Parse(cachedResponse);
                        return Ok(new { 
                            success = true, 
                            dashboardConfig = parsedResponse.ToString(),
                            message = "Dashboard configuration loaded from cached data",
                            cached = true
                        });
                    }
                    catch (JsonReaderException)
                    {
                        // If cached JSON is invalid, continue to API call
                    }
                }
                
                // Get phone number from static mapping
                if (!UserPhoneMapping.TryGetValue(request.UserId, out string phoneNumber))
                {
                    return BadRequest(new { error = "User not found in phone mapping" });
                }

                var url = "http://localhost:8000/run";
                object dashboardConfig = null;
                bool apiCallSuccessful = false;
                string responseContent = "";
                
                var body = new
                {
                    appName = "lakshmai-agent",
                    userId = request.UserId,
                    sessionId = request.SessionId,
                    newMessage = new
                    {
                        parts = new[]
                        {
                            new
                            {
                                text = JsonConvert.SerializeObject(new
                                {
                                    RequestType = "HomeDashboard",
                                    Request = new
                                    {
                                        PhoneNumber = phoneNumber,
                                        sessionID = request.SessionId,
                                        userId = request.UserId
                                    }
                                })
                            }
                        },
                        role = "user"
                    },
                    streaming = false,
                    stateDelta = new { additionalProp1 = new { } }
                };

                var jsonBody = JsonConvert.SerializeObject(body);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
                        httpRequest.Headers.Add("accept", "application/json");
                        httpRequest.Content = content;

                        var response = await httpClient.SendAsync(httpRequest);
                        responseContent = await response.Content.ReadAsStringAsync();

                        var responseArray = JArray.Parse(responseContent);
                        
                        // Find the last response that contains dashboard configuration
                        JObject parsedConfig = null;
                        for (int i = responseArray.Count - 1; i >= 0; i--)
                        {
                            var item = responseArray[i];
                            if (item["content"]?["parts"] != null)
                            {
                                foreach (var part in item["content"]["parts"])
                                {
                                    if (part["text"] != null)
                                    {
                                        var text = part["text"].ToString();
                                        
                                        // Look for JSON content that contains dashboard configuration
                                        if (text.Contains("TextResp") || text.Contains("ChartConfigResp") || text.Contains("ToDoResp"))
                                        {
                                            try
                                            {
                                                // Extract JSON from the text (it's wrapped in ```json ... ```)
                                                var jsonStart = text.IndexOf("```json");
                                                var jsonEnd = text.LastIndexOf("```");
                                                if (jsonStart >= 0 && jsonEnd > jsonStart)
                                                {
                                                    var jsonContent = text.Substring(jsonStart + 7, jsonEnd - jsonStart - 7).Trim();
                                                    var responseData = JObject.Parse(jsonContent);
                                                    
                                                    // Create dashboard configuration from the response data
                                                    parsedConfig = new JObject
                                                    {
                                                        ["dashboardConfig"] = new JObject
                                                        {
                                                            ["theme"] = "light",
                                                            ["currency"] = "₹",
                                                            ["language"] = "en-IN",
                                                            ["lastUpdated"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
                                                        },
                                                        ["dashboardWidgets"] = new JArray
                                                        {
                                                            new JObject
                                                            {
                                                                ["id"] = "financialProjection",
                                                                ["title"] = "Financial Projection",
                                                                ["type"] = "chart",
                                                                ["chartType"] = "line",
                                                                ["chartData"] = responseData["ChartConfigResp"]?["chartConfig"]?["data"] ?? new JObject(),
                                                                ["currentValue"] = 750726,
                                                                ["currency"] = "₹",
                                                                ["changePercentage"] = 0.05,
                                                                ["changeType"] = "positive"
                                                            },
                                                            new JObject
                                                            {
                                                                ["id"] = "textResponse",
                                                                ["title"] = "Analysis",
                                                                ["type"] = "text",
                                                                ["content"] = responseData["TextResp"]?.ToString() ?? "",
                                                                ["currentValue"] = 0,
                                                                ["currency"] = "",
                                                                ["changePercentage"] = 0,
                                                                ["changeType"] = "neutral"
                                                            },
                                                            new JObject
                                                            {
                                                                ["id"] = "todoList",
                                                                ["title"] = "Action Items",
                                                                ["type"] = "list",
                                                                ["items"] = responseData["ToDoResp"] ?? new JArray(),
                                                                ["currentValue"] = 0,
                                                                ["currency"] = "",
                                                                ["changePercentage"] = 0,
                                                                ["changeType"] = "neutral"
                                                            }
                                                        },
                                                        ["userProfile"] = new JObject
                                                        {
                                                            ["name"] = "John Doe",
                                                            ["phoneNumber"] = "9876543210",
                                                            ["email"] = "john.doe@example.com",
                                                            ["address"] = "123 Main St, Anytown",
                                                            ["dob"] = "1990-01-01"
                                                        },
                                                        ["notifications"] = new JArray
                                                        {
                                                            new JObject
                                                            {
                                                                ["id"] = "notification1",
                                                                ["type"] = "info",
                                                                ["message"] = "Financial projection analysis completed.",
                                                                ["date"] = DateTime.UtcNow.ToString("yyyy-MM-dd")
                                                            }
                                                        },
                                                        ["quickActions"] = new JArray
                                                        {
                                                            new JObject
                                                            {
                                                                ["id"] = "quickAction1",
                                                                ["label"] = "View Projection",
                                                                ["icon"] = "chart"
                                                            },
                                                            new JObject
                                                            {
                                                                ["id"] = "quickAction2",
                                                                ["label"] = "Update Goals",
                                                                ["icon"] = "target"
                                                            },
                                                            new JObject
                                                            {
                                                                ["id"] = "quickAction3",
                                                                ["label"] = "Track Progress",
                                                                ["icon"] = "progress"
                                                            }
                                                        },
                                                        ["dataSummary"] = new JObject
                                                        {
                                                            ["totalAssets"] = 794629,
                                                            ["totalLiabilities"] = 75000,
                                                            ["netWorth"] = 750726,
                                                            ["creditScore"] = 746,
                                                            ["totalTransactions"] = 100,
                                                            ["investmentReturns"] = 0.05,
                                                            ["epfBalance"] = 211111,
                                                            ["stockHoldings"] = 200642,
                                                            ["mutualFundValue"] = 177605,
                                                            ["savingsBalance"] = 195297,
                                                            ["usSecuritiesValue"] = 30071,
                                                            ["creditCardOutstanding"] = 75000,
                                                            ["totalCreditAccounts"] = 6,
                                                            ["activeCreditAccounts"] = 6,
                                                            ["lastDataUpdate"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
                                                        }
                                                    };
                                                    
                                                    dashboardConfig = parsedConfig;
                                                    break; // Found the config, exit the loop
                                                }
                                            }
                                            catch (JsonReaderException)
                                            {
                                                // Continue searching for valid JSON
                                                continue;
                                            }
                                        }
                                    }
                                }
                                if (parsedConfig != null)
                                    break; // Found the config, exit the outer loop
                            }
                        }

                        if (parsedConfig != null)
                        {
                            apiCallSuccessful = true;
                            dashboardConfig = parsedConfig.ToObject<object>();
                            // Update cache with successful result
                            UpdateCache(request.UserId, dashboardConfig);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception but continue to fallback
                    Console.WriteLine($"API call failed: {ex.Message}");
                }
                
                // Wait for 1 second to see if we get a successful result
                await Task.Delay(1000);
                
                // If API call was successful, return the result
                if (apiCallSuccessful && dashboardConfig != null)
                {
                    return Ok(new { 
                        success = true, 
                        dashboardConfig = dashboardConfig,
                        message = "Dashboard configuration loaded successfully",
                        cached = false
                    });
                }
                
                // If API call failed or timed out, try to return cached data
                if (CacheData.CachedDashboardResponses.TryGetValue(request.UserId, out string fallbackResponse))
                {
                    try
                    {
                        var parsedFallback = JObject.Parse(fallbackResponse);
                        return Ok(new { 
                            success = true, 
                            dashboardConfig = parsedFallback,
                            message = "Dashboard configuration loaded from fallback cache",
                            cached = true,
                            fallback = true
                        });
                    }
                    catch (JsonReaderException)
                    {
                        // If cached JSON is invalid, continue to error
                    }
                }
                
                // If no cached data available, return error
                return Ok(new { 
                    success = false, 
                    error = "Dashboard configuration not found in response and no cached data available",
                    response = responseContent 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    success = false, 
                    error = "Internal server error", 
                    message = ex.Message 
                });
            }
        }
        
        /// <summary>
        /// Updates the cache with new dashboard configuration for a user
        /// </summary>
        /// <param name="userId">The user ID</param>
        /// <param name="dashboardConfig">The dashboard configuration to cache</param>
        private void UpdateCache(string userId, object dashboardConfig)
        {
            try
            {
                // Convert to JSON string and update the cache
                string jsonConfig = JsonConvert.SerializeObject(dashboardConfig);
                CacheData.CachedDashboardResponses[userId] = jsonConfig;
                
                // Also update the runtime cache if needed
                DashboardCache[userId] = new CachedDashboardResponse
                {
                    DashboardConfig = dashboardConfig,
                    CachedAt = DateTime.UtcNow
                };
                
                Console.WriteLine($"Cache updated for user: {userId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating cache for user {userId}: {ex.Message}");
            }
        }
    }

    public class ChatMessageRequest
    {
        public string Text { get; set; }
        public string UserId { get; set; }
        public string SessionId { get; set; }
    }

    public class LoadHomeDashboardRequest
    {
        public string UserId { get; set; }
        public string SessionId { get; set; }
    }
    
    public class CachedDashboardResponse
    {
        public object DashboardConfig { get; set; }
        public DateTime CachedAt { get; set; }
    }
} 