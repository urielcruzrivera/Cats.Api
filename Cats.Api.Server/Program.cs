using Cats.Application.UseCases.GetBreeds;
using Cats.Application.UseCases.GetImages;
using Cats.Infrastructure;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<GetBreedsHandler>();
builder.Services.AddScoped<GetImagesHandler>();

builder.Services.AddInfrastructure(builder.Configuration);

const string CorsPolicy = "AngularClient";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: CorsPolicy, policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:60490",
                "https://localhost:60490",
                "https://127.0.0.1:60490"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseCors(CorsPolicy);


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();


var angularDistPath = Path.Combine(
    app.Environment.ContentRootPath,
    "dist", "cats.api.client", "browser"
);

if (Directory.Exists(angularDistPath))
{
    var fileProvider = new PhysicalFileProvider(angularDistPath);

    app.UseDefaultFiles(new DefaultFilesOptions
    {
        FileProvider = fileProvider
    });

    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = fileProvider
    });


    app.MapFallback(async context =>
    {
        var path = context.Request.Path.Value ?? "";


        if (path.StartsWith("/api", StringComparison.OrdinalIgnoreCase) ||
            path.StartsWith("/swagger", StringComparison.OrdinalIgnoreCase))
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            return;
        }

        context.Response.ContentType = "text/html";
        await context.Response.SendFileAsync(Path.Combine(angularDistPath, "index.html"));
    });
}
else
{
    app.MapGet("/", () => "Angular dist not found. Build the client and try again.");
}

app.Run();
