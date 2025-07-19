using Newtonsoft.Json;

namespace SemanticPen.SDK.Models
{
    public class Article
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("progress")]
        public int Progress { get; set; }

        [JsonProperty("targetKeyword")]
        public string TargetKeyword { get; set; }

        [JsonProperty("projectName")]
        public string ProjectName { get; set; }

        [JsonProperty("createdAt")]
        public string CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public string UpdatedAt { get; set; }

        public bool IsCompleted => Status == "completed";
        public bool IsInProgress => Status == "in_progress" || Status == "processing";
        public bool IsFailed => Status == "failed" || Status == "error";
    }
}