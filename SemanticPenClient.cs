using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SemanticPen.SDK.Models;
using SemanticPen.SDK.Exceptions;

namespace SemanticPen.SDK
{
    public class SemanticPenClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly SemanticPenConfiguration _configuration;
        private bool _disposed = false;

        public SemanticPenClient(string apiKey, string baseUrl = "https://www.semanticpen.com")
            : this(new SemanticPenConfiguration(apiKey, baseUrl))
        {
        }

        public SemanticPenClient(SemanticPenConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            
            _httpClient = new HttpClient();
            _httpClient.Timeout = _configuration.Timeout;
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_configuration.ApiKey}");
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "SemanticPen-CSharp-SDK/1.0.0");
        }

        public async Task<GenerateArticleResponse> GenerateArticleAsync(string targetKeyword, string projectName = null)
        {
            if (string.IsNullOrWhiteSpace(targetKeyword))
                throw SemanticPenException.ValidationError("Target keyword cannot be null or empty");

            var request = new GenerateArticleRequest(targetKeyword.Trim(), projectName);
            var jsonContent = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync($"{_configuration.BaseUrl}/api/articles", httpContent);
                return await HandleResponseAsync<GenerateArticleResponse>(response);
            }
            catch (HttpRequestException ex)
            {
                throw SemanticPenException.NetworkError($"Failed to send request: {ex.Message}", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw SemanticPenException.NetworkError("Request timed out", ex);
            }
        }

        public async Task<Article> GetArticleAsync(string articleId)
        {
            if (string.IsNullOrWhiteSpace(articleId))
                throw SemanticPenException.ValidationError("Article ID cannot be null or empty");

            try
            {
                var response = await _httpClient.GetAsync($"{_configuration.BaseUrl}/api/articles/{articleId.Trim()}");
                return await HandleResponseAsync<Article>(response);
            }
            catch (HttpRequestException ex)
            {
                throw SemanticPenException.NetworkError($"Failed to send request: {ex.Message}", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw SemanticPenException.NetworkError("Request timed out", ex);
            }
        }

        private async Task<T> HandleResponseAsync<T>(HttpResponseMessage response)
        {
            var responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return JsonConvert.DeserializeObject<T>(responseBody);
                }
                catch (JsonException ex)
                {
                    throw SemanticPenException.ApiError($"Failed to parse response: {ex.Message}", (int)response.StatusCode);
                }
            }

            string errorMessage = responseBody;
            try
            {
                var errorObj = JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(responseBody);
                if (errorObj?["message"] != null)
                    errorMessage = errorObj["message"].ToString();
                else if (errorObj?["error"] != null)
                    errorMessage = errorObj["error"].ToString();
            }
            catch
            {
            }

            var statusCode = (int)response.StatusCode;
            switch (statusCode)
            {
                case 401:
                    throw SemanticPenException.AuthenticationError(errorMessage);
                case 404:
                    throw SemanticPenException.NotFoundError(errorMessage);
                default:
                    throw SemanticPenException.ApiError(errorMessage, statusCode);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _httpClient?.Dispose();
                _disposed = true;
            }
        }
    }
}