using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.Domain.Abstractions
{
    public interface ICatApiClient
    {
        Task<IReadOnlyList<Breed>> GetBreedsAsync(CancellationToken ct);
        Task<IReadOnlyList<CatImage>> SearchImagesAsync(string breedId, int limit, CancellationToken ct);
    }

    public record Breed(string Id, string Name);
    public record CatImage(string Id, string Url);
}
