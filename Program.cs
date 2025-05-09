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

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
       {
           policy.WithOrigins("http://localhost:5173") // React port
                 .AllowAnyHeader()
                 .AllowAnyMethod();
       });
});


var app = builder.Build();

app.UseCors();

#region DeckEndpoints

app.MapGet("/decks", (IFlashcardRepository repo) =>
{
    return repo.GetAllDecks();
});

app.MapGet("/decks/{id}", (Guid id, IFlashcardRepository repo) =>
{
    var deck = repo.GetDeck(id);
    return deck is not null ? Results.Ok(deck) : Results.NotFound();
});

app.MapGet("decks/{deckId}/cards", (Guid deckId, IFlashcardRepository repo) =>
{
    var cards = repo.GetCards(deckId);
    return Results.Ok(cards);
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

app.MapPut("/decks/{deckId}", (Guid deckId, Deck updatedDeck, IFlashcardRepository repo) =>
{
    var currentDeck = repo.UpdateDeck(deckId, updatedDeck);
    return Results.Ok(currentDeck);
});

app.MapDelete("/decks/{id}", async (Guid id, AppDbContext db) =>
{
    var deck = await db.Decks.FindAsync(id);
    if (deck is null) return Results.NotFound();

    deck.Status = 0;
    deck.LastUpdateDate = DateTime.UtcNow;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

#endregion

app.Run();
