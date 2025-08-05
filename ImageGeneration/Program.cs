using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
builder.Services.AddSingleton<StableDiffusionService>();
var app = builder.Build();
app.UseCors();

app.MapPost("/generate", async ([FromBody] GenerateRequest request, StableDiffusionService sdService) =>
{
    if (string.IsNullOrWhiteSpace(request?.Prompt))
    {
        return Results.BadRequest("Prompt cannot be empty.");
    }

    ;

    using var image = await Task.Run(() => sdService.GenerateImage(request.Prompt));
    var memoryStream = new MemoryStream();
    await image.SaveAsPngAsync(memoryStream);
    memoryStream.Position = 0;

    var safeFileName = new string(request.Prompt.Take(20).Where(c => !Path.GetInvalidFileNameChars().Contains(c)).ToArray());
    return Results.File(memoryStream, "image/png", $"image-for-{safeFileName}.png");
});

app.Run();

public record GenerateRequest
{
    public string? Prompt { get; set; }
}
