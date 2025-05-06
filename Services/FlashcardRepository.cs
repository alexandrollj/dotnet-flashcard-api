using FlashcardApi.Models;
using FlashcardApi.Data;
using Microsoft.EntityFrameworkCore;

namespace FlashcardApi.Services;

public interface IFlashcardRepository
{
    IEnumerable<Deck> GetAllDecks();
    Deck? GetDeck(Guid id);
    Deck CreateDeck(Deck deck);
    Card AddCard(Guid deckId, Card card);
    IEnumerable<Card> GetCards(Guid deckId);
}

public class FlashcardRepository : IFlashcardRepository
{
    private readonly AppDbContext _context;

    public FlashcardRepository(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Deck> GetAllDecks()
    {
        return _context.Decks.Include(d => d.Cards).ToList();
    }

    public Deck? GetDeck(Guid id)
    {
        return _context.Decks.Include(d => d.Cards).FirstOrDefault(d => d.Id == id);
    }

    public Deck CreateDeck(Deck deck)
    {
        _context.Decks.Add(deck);
        _context.SaveChanges();
        return deck;
    }

    public Card AddCard(Guid deckId, Card card)
    {
        card.DeckId = deckId;
        _context.Cards.Add(card);
        _context.SaveChanges();
        return card;
    }

    public IEnumerable<Card> GetCards(Guid deckId)
    {
        return _context.Cards.Where(c => c.DeckId == deckId).ToList();
    }
}
