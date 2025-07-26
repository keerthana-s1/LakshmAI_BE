using System;
using System.Collections.Generic;

namespace BELakshmai.Data
{
    public static class CacheData
    {
        public static readonly Dictionary<string, string> CachedDashboardResponses = new Dictionary<string, string>
        {
            ["user"] = @"{
                ""dashboardConfig"": {
                    ""theme"": ""light"",
                    ""currency"": ""₹"",
                    ""language"": ""en-IN"",
                    ""lastUpdated"": """ + DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ") + @"""
                },
                ""dashboardWidgets"": [
                    {
                        ""id"": ""bankAccounts"",
                        ""title"": ""Bank Accounts"",
                        ""type"": ""card"",
                        ""chartType"": ""line"",
                        ""chartData"": {
                            ""labels"": [""2024-07-22"", ""2024-07-23"", ""2024-07-24"", ""2024-07-25"", ""2024-07-26"", ""2024-07-27"", ""2024-07-28""],
                            ""datasets"": [
                                {
                                    ""label"": ""Account Balance"",
                                    ""data"": [10000, 10500, 11000, 10800, 11200, 11500, 12000],
                                    ""borderColor"": ""#2563EB"",
                                    ""fill"": false
                                }
                            ]
                        },
                        ""currentValue"": 195297,
                        ""currency"": ""₹"",
                        ""changePercentage"": 0.05,
                        ""changeType"": ""positive""
                    },
                    {
                        ""id"": ""mutualFunds"",
                        ""title"": ""Mutual Funds"",
                        ""type"": ""card"",
                        ""chartType"": ""line"",
                        ""chartData"": {
                            ""labels"": [""2024-07-22"", ""2024-07-23"", ""2024-07-24"", ""2024-07-25"", ""2024-07-26"", ""2024-07-27"", ""2024-07-28""],
                            ""datasets"": [
                                {
                                    ""label"": ""Investment Value"",
                                    ""data"": [5000, 5100, 5200, 5300, 5400, 5500, 5600],
                                    ""borderColor"": ""#10B981"",
                                    ""fill"": false
                                }
                            ]
                        },
                        ""currentValue"": 177605,
                        ""currency"": ""₹"",
                        ""changePercentage"": 0.03,
                        ""changeType"": ""positive""
                    },
                    {
                        ""id"": ""stocks"",
                        ""title"": ""Stocks"",
                        ""type"": ""card"",
                        ""chartType"": ""line"",
                        ""chartData"": {
                            ""labels"": [""2024-07-22"", ""2024-07-23"", ""2024-07-24"", ""2024-07-25"", ""2024-07-26"", ""2024-07-27"", ""2024-07-28""],
                            ""datasets"": [
                                {
                                    ""label"": ""Investment Value"",
                                    ""data"": [2500, 2550, 2600, 2650, 2700, 2750, 2800],
                                    ""borderColor"": ""#EAB308"",
                                    ""fill"": false
                                }
                            ]
                        },
                        ""currentValue"": 200642,
                        ""currency"": ""₹"",
                        ""changePercentage"": 0.02,
                        ""changeType"": ""positive""
                    },
                    {
                        ""id"": ""usStocks"",
                        ""title"": ""US Stocks"",
                        ""type"": ""card"",
                        ""chartType"": ""line"",
                        ""chartData"": {
                            ""labels"": [""2024-07-22"", ""2024-07-23"", ""2024-07-24"", ""2024-07-25"", ""2024-07-26"", ""2024-07-27"", ""2024-07-28""],
                            ""datasets"": [
                                {
                                    ""label"": ""Investment Value"",
                                    ""data"": [1000, 1010, 1020, 1030, 1040, 1050, 1060],
                                    ""borderColor"": ""#F43F5E"",
                                    ""fill"": false
                                }
                            ]
                        },
                        ""currentValue"": 30071,
                        ""currency"": ""₹"",
                        ""changePercentage"": 0.01,
                        ""changeType"": ""positive""
                    },
                    {
                        ""id"": ""creditCards"",
                        ""title"": ""Credit Cards"",
                        ""type"": ""card"",
                        ""chartType"": ""line"",
                        ""chartData"": {
                            ""labels"": [""2024-07-22"", ""2024-07-23"", ""2024-07-24"", ""2024-07-25"", ""2024-07-26"", ""2024-07-27"", ""2024-07-28""],
                            ""datasets"": [
                                {
                                    ""label"": ""Outstanding Balance"",
                                    ""data"": [5000, 17000, 14000, 8000, 11000, 20000, 5000],
                                    ""borderColor"": ""#6366F1"",
                                    ""fill"": false
                                }
                            ]
                        },
                        ""currentValue"": 75000,
                        ""currency"": ""₹"",
                        ""changePercentage"": 0.05,
                        ""changeType"": ""negative""
                    },
                    {
                        ""id"": ""netWorth"",
                        ""title"": ""Net Worth"",
                        ""type"": ""card"",
                        ""chartType"": ""line"",
                        ""chartData"": {
                            ""labels"": [""2024-07-22"", ""2024-07-23"", ""2024-07-24"", ""2024-07-25"", ""2024-07-26"", ""2024-07-27"", ""2024-07-28""],
                            ""datasets"": [
                                {
                                    ""label"": ""Net Worth"",
                                    ""data"": [18000, 18640, 19260, 19740, 20480, 21300, 22140],
                                    ""borderColor"": ""#064E3B"",
                                    ""fill"": false
                                }
                            ]
                        },
                        ""currentValue"": 750726,
                        ""currency"": ""₹"",
                        ""changePercentage"": 0.04,
                        ""changeType"": ""positive""
                    },
                    {
                        ""id"": ""creditScore"",
                        ""title"": ""Credit Score"",
                        ""type"": ""card"",
                        ""currentValue"": 746,
                        ""currency"": """",
                        ""changePercentage"": 0,
                        ""changeType"": ""positive""
                    },
                    {
                        ""id"": ""epfBalance"",
                        ""title"": ""EPF Balance"",
                        ""type"": ""card"",
                        ""currentValue"": 211111,
                        ""currency"": ""₹"",
                        ""changePercentage"": 0.01,
                        ""changeType"": ""positive""
                    },
                    {
                        ""id"": ""expenseBreakdown"",
                        ""title"": ""Expense Breakdown"",
                        ""type"": ""chart"",
                        ""dashboard"": ""homeDashboard"",
                        ""chartType"": ""doughnut"",
                        ""chartData"": {
                            ""labels"": [""Food"", ""Travel"", ""Entertainment"", ""Shopping""],
                            ""datasets"": [
                                {
                                    ""data"": [3000, 1500, 1000, 2000],
                                    ""backgroundColor"": [""#EF4444"", ""#3B82F6"", ""#10B981"", ""#EAB308""],
                                    ""hoverBackgroundColor"": [""#EF4444"", ""#3B82F6"", ""#10B981"", ""#EAB308""]
                                }
                            ]
                        }
                    },
                    {
                        ""id"": ""incomeVsExpense"",
                        ""title"": ""Income vs Expense"",
                        ""type"": ""chart"",
                        ""dashboard"": ""homeDashboard"",
                        ""chartType"": ""bar"",
                        ""chartData"": {
                            ""labels"": [""July""],
                            ""datasets"": [
                                {
                                    ""label"": ""Income"",
                                    ""data"": [10000],
                                    ""backgroundColor"": ""#10B981""
                                },
                                {
                                    ""label"": ""Expense"",
                                    ""data"": [7500],
                                    ""backgroundColor"": ""#EF4444""
                                }
                            ]
                        }
                    },
                    {
                        ""id"": ""investmentPortfolio"",
                        ""title"": ""Investment Portfolio"",
                        ""type"": ""chart"",
                        ""dashboard"": ""homeDashboard"",
                        ""chartType"": ""pie"",
                        ""chartData"": {
                            ""labels"": [""Mutual Funds"", ""Stocks"", ""US Stocks""],
                            ""datasets"": [
                                {
                                    ""data"": [177605, 200642, 30071],
                                    ""backgroundColor"": [""#10B981"", ""#EAB308"", ""#F43F5E""],
                                    ""hoverBackgroundColor"": [""#10B981"", ""#EAB308"", ""#F43F5E""]
                                }
                            ]
                        }
                    }
                ],
                ""userProfile"": {
                    ""name"": ""John Doe"",
                    ""phoneNumber"": ""9876543210"",
                    ""email"": ""john.doe@example.com"",
                    ""address"": ""123 Main St, Anytown"",
                    ""dob"": ""1990-01-01""
                },
                ""notifications"": [
                    {
                        ""id"": ""notification1"",
                        ""type"": ""info"",
                        ""message"": ""Your credit card bill is due soon."",
                        ""date"": ""2024-07-31""
                    },
                    {
                        ""id"": ""notification2"",
                        ""type"": ""alert"",
                        ""message"": ""Low balance in your savings account."",
                        ""date"": ""2024-07-30""
                    }
                ],
                ""quickActions"": [
                    {
                        ""id"": ""quickAction1"",
                        ""label"": ""Pay Bills"",
                        ""icon"": ""bill""
                    },
                    {
                        ""id"": ""quickAction2"",
                        ""label"": ""Transfer Money"",
                        ""icon"": ""transfer""
                    },
                    {
                        ""id"": ""quickAction3"",
                        ""label"": ""Invest"",
                        ""icon"": ""invest""
                    }
                ],
                ""dataSummary"": {
                    ""totalAssets"": 794629,
                    ""totalLiabilities"": 75000,
                    ""netWorth"": 750726,
                    ""creditScore"": 746,
                    ""totalTransactions"": 100,
                    ""investmentReturns"": 0.05,
                    ""epfBalance"": 211111,
                    ""stockHoldings"": 200642,
                    ""mutualFundValue"": 177605,
                    ""savingsBalance"": 195297,
                    ""usSecuritiesValue"": 30071,
                    ""creditCardOutstanding"": 75000,
                    ""totalCreditAccounts"": 6,
                    ""activeCreditAccounts"": 6,
                    ""lastDataUpdate"": """ + DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ") + @"""
                }
            }",
            ["user1"] = @"{
                ""dashboardConfig"": {
                    ""theme"": ""dark"",
                    ""currency"": ""₹"",
                    ""language"": ""en-IN"",
                    ""lastUpdated"": """ + DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ") + @"""
                },
                ""dashboardWidgets"": [
                    {
                        ""id"": ""bankAccounts"",
                        ""title"": ""Bank Accounts"",
                        ""type"": ""card"",
                        ""currentValue"": 50000,
                        ""currency"": ""₹"",
                        ""changePercentage"": 0.02,
                        ""changeType"": ""positive""
                    }
                ],
                ""userProfile"": {
                    ""name"": ""Jane Smith"",
                    ""phoneNumber"": ""1111111111"",
                    ""email"": ""jane.smith@example.com"",
                    ""address"": ""456 Oak St, Somewhere"",
                    ""dob"": ""1985-05-15""
                },
                ""notifications"": [],
                ""quickActions"": [
                    {
                        ""id"": ""quickAction1"",
                        ""label"": ""Pay Bills"",
                        ""icon"": ""bill""
                    }
                ],
                ""dataSummary"": {
                    ""totalAssets"": 50000,
                    ""totalLiabilities"": 0,
                    ""netWorth"": 50000,
                    ""creditScore"": 750,
                    ""totalTransactions"": 50,
                    ""investmentReturns"": 0.03,
                    ""epfBalance"": 100000,
                    ""stockHoldings"": 0,
                    ""mutualFundValue"": 0,
                    ""savingsBalance"": 50000,
                    ""usSecuritiesValue"": 0,
                    ""creditCardOutstanding"": 0,
                    ""totalCreditAccounts"": 0,
                    ""activeCreditAccounts"": 0,
                    ""lastDataUpdate"": """ + DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ") + @"""
                }
            }",
            ["user2"] = @"{
                ""dashboardConfig"": {
                    ""theme"": ""light"",
                    ""currency"": ""₹"",
                    ""language"": ""en-IN"",
                    ""lastUpdated"": """ + DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ") + @"""
                },
                ""dashboardWidgets"": [
                    {
                        ""id"": ""bankAccounts"",
                        ""title"": ""Bank Accounts"",
                        ""type"": ""card"",
                        ""currentValue"": 75000,
                        ""currency"": ""₹"",
                        ""changePercentage"": 0.08,
                        ""changeType"": ""positive""
                    }
                ],
                ""userProfile"": {
                    ""name"": ""Bob Johnson"",
                    ""phoneNumber"": ""2222222222"",
                    ""email"": ""bob.johnson@example.com"",
                    ""address"": ""789 Pine St, Elsewhere"",
                    ""dob"": ""1988-12-20""
                },
                ""notifications"": [],
                ""quickActions"": [
                    {
                        ""id"": ""quickAction1"",
                        ""label"": ""Pay Bills"",
                        ""icon"": ""bill""
                    }
                ],
                ""dataSummary"": {
                    ""totalAssets"": 75000,
                    ""totalLiabilities"": 10000,
                    ""netWorth"": 65000,
                    ""creditScore"": 720,
                    ""totalTransactions"": 75,
                    ""investmentReturns"": 0.04,
                    ""epfBalance"": 150000,
                    ""stockHoldings"": 0,
                    ""mutualFundValue"": 0,
                    ""savingsBalance"": 75000,
                    ""usSecuritiesValue"": 0,
                    ""creditCardOutstanding"": 10000,
                    ""totalCreditAccounts"": 1,
                    ""activeCreditAccounts"": 1,
                    ""lastDataUpdate"": """ + DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ") + @"""
                }
            }"
        };
    }
} 