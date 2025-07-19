using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace SemanticPen.SDK.Models
{
    public class GenerateArticleResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("articleIds")]
        public List<string> ArticleIds { get; set; }

        [JsonProperty("articleId")]
        public string ArticleId { get; set; }

        public bool HasArticleIds => (ArticleIds != null && ArticleIds.Any()) || !string.IsNullOrEmpty(ArticleId);

        public string GetFirstArticleId()
        {
            if (!string.IsNullOrEmpty(ArticleId))
                return ArticleId;
            
            return ArticleIds?.FirstOrDefault();
        }
    }
}