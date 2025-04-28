using McpModelsDemo.Models;
using McpModelsDemo.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddHttpClient<IModelActions, ModelActions>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapPost("/mcp/query", async (McpQueryRequest request, IModelActions actions) =>
{
    var modelResponse = await actions.RouteQueryAsync(request.Query);
    return Results.Ok(new { Response = modelResponse });
})
.WithName("McpQuery");

app.Run();
