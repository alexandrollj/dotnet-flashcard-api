using FlashcardApi.Models;
using FlashcardApi.Data;
using Microsoft.EntityFrameworkCore;

namespace FlashcardApi.Services;

public interface IFlashcardRepository
{
    IEnumerable<Deck> GetAllDecks();
    Deck? GetDeck(Guid id);
    Deck CreateDeck(Deck deck);
    Deck? UpdateDeck(Guid id, Deck deck);
    Deck? DeleteDeck(Guid id);
    Card AddCard(Guid deckId, Card card);
    IEnumerable<Card> GetCards(Guid deckId);
    Card? GetCard(Guid deckId, Guid cardId);
    Card? UpdateCard(Guid deckId, Guid cardId, Card updatedCard);
    Card? DeleteCard(Guid cardId);

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
        return _context.Decks
            .Where(d => d.Status == 1)
            .Include(d => d.Cards).ToList();

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

    public Deck? UpdateDeck(Guid deckId, Deck updatedDeck)
    {
        var currentDeck = _context.Decks.FirstOrDefault(d => d.Id == deckId && d.Status == 1);

        if (currentDeck == null)
            return null;

        currentDeck.Name = updatedDeck.Name;
        currentDeck.LastUpdateDate = DateTime.UtcNow;

        _context.SaveChanges();
        return currentDeck;
    }

    public Deck? DeleteDeck(Guid deckId)
    {
        var deckToDelete = _context.Decks.FirstOrDefault(d => d.Id == deckId);

        if (deckToDelete == null)
            return null;

        deckToDelete.Status = 0;

        _context.SaveChanges();
        return deckToDelete;
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

    public Card? GetCard(Guid deckId, Guid cardId)
    {

        return _context.Cards.SingleOrDefault(c => c.Id == cardId && c.DeckId == deckId);
    }

    public Card? UpdateCard(Guid deckId, Guid cardId, Card updatedCard)
    {
        var currentCard = _context.Cards.FirstOrDefault(c => c.Id == cardId && c.Status == 1);
        if (currentCard == null)
            return null;

        currentCard.Question = updatedCard.Question;
        currentCard.Answer = updatedCard.Answer;
        currentCard.LastUpdateDate = DateTime.UtcNow;

        _context.SaveChanges();
        return currentCard;
    }

    public Card? DeleteCard(Guid cardId)
    {

        var cardToDelete = _context.Cards.SingleOrDefault(c => c.Id == cardId);

        if (cardToDelete == null)
            return null;

        cardToDelete.Status = 0;
        _context.SaveChanges();
        return cardToDelete;
    }
}
