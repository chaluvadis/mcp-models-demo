using System.ComponentModel.DataAnnotations;

namespace McpModelsDemo.Models;

public record McpQueryRequest(
    [property: Required]
    [property: MinLength(3)]
    string Query
);

public record ModelApiConfig(string ApiKey = "", string Endpoint = "");
