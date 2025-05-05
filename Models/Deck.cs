namespace FlashcardApi.Models;

public class Deck
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public List<Card> Cards { get; set; } = new();
}
