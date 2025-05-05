using FlashcardApi.Models;
using FlashcardApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlashcardApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DecksController : ControllerBase
{
    private readonly IFlashcardRepository _repo;
    public DecksController(IFlashcardRepository repo) => _repo = repo;

    [HttpGet]
    public ActionResult<IEnumerable<Deck>> Get() =>
        Ok(_repo.GetAllDecks());

    [HttpGet("{id:guid}")]
    public ActionResult<Deck> Get(Guid id)
    {
        var deck = _repo.GetDeck(id);
        return deck is not null ? Ok(deck) : NotFound();
    }

    [HttpPost]
    public ActionResult<Deck> Post([FromBody] Deck deck)
    {
        var created = _repo.CreateDeck(deck);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }
}
