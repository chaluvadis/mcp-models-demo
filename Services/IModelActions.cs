namespace McpModelsDemo.Services;

public interface IModelActions
{
    ValueTask<string> RouteQueryAsync(string query);
}