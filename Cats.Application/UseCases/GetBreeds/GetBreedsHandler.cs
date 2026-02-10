using Cats.Application.Dtos;
using Cats.Domain.Abstractions;

namespace Cats.Application.UseCases.GetBreeds;

public class GetBreedsHandler(ICatApiClient client)
{
    public async Task<IReadOnlyList<CatBreedDto>> Handle(GetBreedsQuery _, CancellationToken ct)
    {
        var breeds = await client.GetBreedsAsync(ct);
        return breeds.OrderBy(b => b.Name).Select(b => new CatBreedDto(b.Id, b.Name)).ToList();
    }
}
