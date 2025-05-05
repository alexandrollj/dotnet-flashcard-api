using FlashcardApi.Models;

namespace FlashcardApi.Services;

public interface IFlashcardRepository
{
    IEnumerable<Deck> GetAllDecks();
    Deck? GetDeck(Guid id);
    Deck CreateDeck(Deck deck);
}

public class FlashcardRepository : IFlashcardRepository
{
    private readonly List<Deck> _decks = new();

    public IEnumerable<Deck> GetAllDecks() => _decks;
    public Deck? GetDeck(Guid id) => _decks.FirstOrDefault(d => d.Id == id);
    public Deck CreateDeck(Deck deck)
    {
        _decks.Add(deck);
        return deck;
    }
}
