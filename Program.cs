using FlashcardApi.Services;
using FlashcardApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
var builder = WebApplication.CreateBuilder(args);


// Register in-memory repository
builder.Services.AddSingleton<IFlashcardRepository, FlashcardRepository>();

var app = builder.Build();


// We add a simpple health-check endpoint
app.MapGet("/", () => Results.Ok("Flashcard API is running!"));

app.MapGet("/decks", (IFlashcardRepository repo) =>
{
    return repo.GetAllDecks();
});

app.MapGet("/decks/{id}", (Guid id, IFlashcardRepository repo) =>
{
    var deck = repo.GetDeck(id);
    return deck is not null ? Results.Ok(deck) : Results.NotFound();
});

app.MapPost("/decks", (Deck deck, IFlashcardRepository repo) =>
{
    var newDeck = repo.CreateDeck(deck);
    return Results.Created($"/decks/{newDeck.Id}", newDeck);
});

app.Run();
