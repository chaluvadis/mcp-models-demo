using McpModelsDemo.Models;

namespace McpModelsDemo.Services;

public class ModelActions(HttpClient httpClient, IConfiguration config) : IModelActions
{
    private readonly ModelApiConfig openAiConfig = config.GetSection("ModelApis:OpenAI").Get<ModelApiConfig>()!;
    private readonly ModelApiConfig grokConfig = config.GetSection("ModelApis:Grok").Get<ModelApiConfig>()!;
    private readonly ModelApiConfig llm2Config = config.GetSection("ModelApis:Llm2").Get<ModelApiConfig>()!;
    private readonly ModelApiConfig gemni3Config = config.GetSection("ModelApis:Gemni3").Get<ModelApiConfig>()!;

    public async ValueTask<string> RouteQueryAsync(string query)
        => query.ToLower() switch
        {
            var q when q.Contains("travel") => await QueryModel(query, openAiConfig, "gpt-3.5-turbo"),
            var q when q.Contains("food") => await QueryModel(query, grokConfig, "grok-1.0"),
            var q when q.Contains("sports") => await QueryModel(query, llm2Config, "llm2-1.0"),
            var q when q.Contains("code") => await QueryModel(query, gemni3Config, "gemni3-1.0"),
            _ => "No suitable model found."
        };

    private async ValueTask<string> QueryModel(string query, ModelApiConfig config, string modelName)
    {
        var requestBody = new
        {
            model = modelName,
            messages = new[] { new { role = "user", content = query } }
        };
        var request = new HttpRequestMessage(HttpMethod.Post, config.Endpoint);
        request.Headers.Add("Authorization", $"Bearer {config.ApiKey}");
        request.Content = JsonContent.Create(requestBody);
        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}
