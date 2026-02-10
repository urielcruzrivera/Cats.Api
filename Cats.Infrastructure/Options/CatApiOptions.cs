namespace Cats.Infrastructure.Options;

public class CatApiOptions
{
    public string BaseUrl { get; init; } = "https://api.thecatapi.com/v1/";
    public string ApiKey { get; init; } = "";
}
