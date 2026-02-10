using Cats.Application.Dtos;
using Cats.Domain.Abstractions;

namespace Cats.Application.UseCases.GetImages;

public class GetImagesHandler(ICatApiClient client)
{
    public async Task<IReadOnlyList<CatImageDto>> Handle(GetImagesQuery q, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(q.BreedId))
            throw new ArgumentException("breedId is required.");

        if (q.Limit < 1 || q.Limit > 50)
            throw new ArgumentException("limit must be between 1 and 50.");

        var images = await client.SearchImagesAsync(q.BreedId, q.Limit, ct);
        return images.Select(i => new CatImageDto(i.Id, i.Url)).ToList();
    }
}
