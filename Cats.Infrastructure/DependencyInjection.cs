using Cats.Domain.Abstractions;
using Cats.Infrastructure.CatApi;
using Cats.Infrastructure.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Cats.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddMemoryCache();
        services.Configure<CatApiOptions>(config.GetSection("CatApi"));

        services.AddHttpClient<ICatApiClient, CatApiClient>((sp, http) =>
        {
            var opt = sp.GetRequiredService<IOptions<CatApiOptions>>().Value;

            http.BaseAddress = new Uri(opt.BaseUrl);
            if (!string.IsNullOrWhiteSpace(opt.ApiKey))
                http.DefaultRequestHeaders.Add("x-api-key", opt.ApiKey);
        });

        return services;
    }
}
