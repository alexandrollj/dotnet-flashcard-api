using FlashcardApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// We add controllers
builder.Services.AddControllers();

// Register in-memory repository
builder.Services.AddSingleton<IFlashcardRepository, FlashcardRepository>();

var app = builder.Build();

// Enable routing to controllers
app.MapControllers();

// We add a simpple health-check endpoint
app.MapGet("/", () => Results.Ok("Flashcard API is running!"));

app.Run();
