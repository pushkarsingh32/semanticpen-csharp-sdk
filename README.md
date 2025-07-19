# SemanticPen C# SDK

Official C# SDK for the SemanticPen API - AI-powered content generation made simple.

## Installation

Install the package via NuGet Package Manager:

```bash
dotnet add package SemanticPen
```

Or via Package Manager Console:

```
Install-Package SemanticPen
```

## Quick Start

```csharp
using SemanticPen.SDK;
using SemanticPen.SDK.Models;

// Initialize the client
var client = new SemanticPenClient("your-api-key");

// Generate an article
var response = await client.GenerateArticleAsync("AI in healthcare", "Medical Blog");

if (response.HasArticleIds)
{
    var articleId = response.GetFirstArticleId();
    
    // Check article status
    var article = await client.GetArticleAsync(articleId);
    Console.WriteLine($"Status: {article.Status} ({article.Progress}%)");
}

// Don't forget to dispose
client.Dispose();
```

## Configuration

You can customize the client configuration:

```csharp
var config = new SemanticPenConfiguration(
    apiKey: "your-api-key",
    baseUrl: "https://www.semanticpen.com",
    timeout: TimeSpan.FromSeconds(60)
);

var client = new SemanticPenClient(config);
```

## Error Handling

The SDK provides specific exception types for different error scenarios:

```csharp
try
{
    var response = await client.GenerateArticleAsync("keyword");
}
catch (SemanticPenException ex)
{
    Console.WriteLine($"Error: {ex.Message}");
    Console.WriteLine($"Error Code: {ex.ErrorCode}");
    
    if (ex.HttpStatusCode.HasValue)
        Console.WriteLine($"HTTP Status: {ex.HttpStatusCode}");
}
```

## API Methods

### Generate Article

```csharp
var response = await client.GenerateArticleAsync("target keyword", "project name");
```

### Get Article Status

```csharp
var article = await client.GetArticleAsync("article-id");
```

## Models

### Article

```csharp
public class Article
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Status { get; set; }
    public int Progress { get; set; }
    public string TargetKeyword { get; set; }
    public string ProjectName { get; set; }
    public string CreatedAt { get; set; }
    public string UpdatedAt { get; set; }
}
```

## Requirements

- .NET Standard 2.0 or higher
- .NET Framework 4.6.1 or higher
- .NET Core 2.0 or higher
- .NET 5.0 or higher

## Support

For support and questions, please visit [SemanticPen API Documentation](https://www.semanticpen.com/api-documentation).

## License

This project is licensed under the MIT License.