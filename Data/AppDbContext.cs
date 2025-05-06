using FlashcardApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FlashcardApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Deck> Decks => Set<Deck>();
    public DbSet<Card> Cards => Set<Card>();
}
