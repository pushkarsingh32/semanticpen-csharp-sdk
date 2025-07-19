using System;
using System.Threading.Tasks;
using SemanticPen.SDK;
using SemanticPen.SDK.Models;
using SemanticPen.SDK.Exceptions;

namespace SemanticPen.SDK.Test
{
    class TestProgram
    {
        static async Task Main(string[] args)
        {
            const string apiKey = "your-api-key-here";
            
            var client = new SemanticPenClient(apiKey);
            
            try
            {
                Console.WriteLine("Testing SemanticPen C# SDK...");
                Console.WriteLine("Generating article...");
                
                var response = await client.GenerateArticleAsync("Simple test article C# SDK");
                
                if (response.HasArticleIds)
                {
                    var articleId = response.GetFirstArticleId();
                    Console.WriteLine($"✓ Article created with ID: {articleId}");
                    
                    Console.WriteLine("Monitoring article progress...");
                    const int maxChecks = 10;
                    
                    for (int i = 1; i <= maxChecks; i++)
                    {
                        await Task.Delay(5000);
                        
                        var article = await client.GetArticleAsync(articleId);
                        Console.WriteLine($"   Check {i}: {article.Status} (progress: {article.Progress}%)");
                        
                        if (article.IsCompleted)
                        {
                            Console.WriteLine("✓ Article completed successfully!");
                            Console.WriteLine($"Title: {article.Title}");
                            break;
                        }
                        
                        if (article.IsFailed)
                        {
                            Console.WriteLine("✗ Article generation failed");
                            break;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No article IDs returned in response");
                }
            }
            catch (SemanticPenException ex)
            {
                Console.WriteLine($"SemanticPen API Error: {ex.Message}");
                Console.WriteLine($"Error Code: {ex.ErrorCode}");
                if (ex.HttpStatusCode.HasValue)
                    Console.WriteLine($"HTTP Status: {ex.HttpStatusCode}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
            finally
            {
                client.Dispose();
            }
        }
    }
}