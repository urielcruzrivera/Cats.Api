using Cats.Application.Dtos;
using Cats.Domain.Abstractions;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Json;

namespace Cats.Infrastructure.CatApi;

public class CatApiClient(HttpClient http, IMemoryCache cache) : ICatApiClient
{
    private class BreedResponse
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
    }
    private class ImageResponse
    {
        public string Id { get; set; } = "";
        public string Url { get; set; } = "";
    }

    public async Task<IReadOnlyList<Breed>> GetBreedsAsync(CancellationToken ct)
    {
        return await cache.GetOrCreateAsync("catapi:breeds", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60);

            var breeds = await http.GetFromJsonAsync<List<BreedResponse>>("breeds", ct) ?? [];
            return breeds.Select(b => new Breed(b.Id, b.Name)).ToList();
        }) ?? [];
    }

    public async Task<IReadOnlyList<CatImage>> SearchImagesAsync(string breedId, int limit, CancellationToken ct)
    {
        var url = $"images/search?breed_ids={Uri.EscapeDataString(breedId)}&limit={limit}";
        var images = await http.GetFromJsonAsync<List<ImageResponse>>(url, ct) ?? [];
        return images.Select(i => new CatImage(i.Id, i.Url)).ToList();
    }
}
