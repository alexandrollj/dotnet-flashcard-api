namespace FlashcardApi.Models;

public class Deck
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public byte Status { get; set; } = 1;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime LastUpdateDate { get; set; } = DateTime.UtcNow;
    public string Name { get; set; } = string.Empty;
    public List<Card> Cards { get; set; } = new();
}
