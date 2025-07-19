using System;

namespace SemanticPen.SDK.Models
{
    public class SemanticPenConfiguration
    {
        public string ApiKey { get; }
        public string BaseUrl { get; }
        public TimeSpan Timeout { get; }

        public SemanticPenConfiguration(string apiKey, string baseUrl = "https://www.semanticpen.com", TimeSpan? timeout = null)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentException("API key cannot be null or empty", nameof(apiKey));

            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new ArgumentException("Base URL cannot be null or empty", nameof(baseUrl));

            ApiKey = apiKey.Trim();
            BaseUrl = baseUrl.TrimEnd('/');
            Timeout = timeout ?? TimeSpan.FromSeconds(30);
        }
    }
}