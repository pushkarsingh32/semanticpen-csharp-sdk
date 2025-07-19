using Newtonsoft.Json;

namespace SemanticPen.SDK.Models
{
    public class GenerateArticleRequest
    {
        [JsonProperty("targetKeyword")]
        public string TargetKeyword { get; set; }

        [JsonProperty("projectName")]
        public string ProjectName { get; set; }

        public GenerateArticleRequest(string targetKeyword, string projectName = null)
        {
            TargetKeyword = targetKeyword;
            ProjectName = projectName;
        }
    }
}