namespace FlashcardApi.Models;

public class Card
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public Guid DeckId { get; set; }
}
