using FlashcardApi.Services;
using FlashcardApi.Models;
using FlashcardApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var builder = WebApplication.CreateBuilder(args);


// Register in-memory repository
builder.Services.AddScoped<IFlashcardRepository, FlashcardRepository>();
builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlite("Data Source=flashcards.db"));

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

app.MapPost("/decks/{deckId}/cards", (Guid deckId, Card card, IFlashcardRepository repo) =>
{
    var added = repo.AddCard(deckId, card);
    return Results.Created($"/decks/{deckId}/cards/{added.Id}", added);
});

app.MapGet("decks/{deckId}/cards", (Guid deckId, IFlashcardRepository repo) =>
{
    var cards = repo.GetCards(deckId);
    return Results.Ok(cards);
});

app.Run();
