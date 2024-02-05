using LookupAPI.Entities;
using LookupAPI.Repositories.GameRepos;
using LookupAPI.Repositories.RecipeRepos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LookupAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private IGameRepository _GameContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public GameController(IGameRepository gameRepository, UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            _GameContext = gameRepository;
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetCount()
        {
            if (_GameContext == null)
            {
                return NotFound();
            }

            return Ok(await _GameContext.CountAsync());
        }

        [HttpGet]
        public async Task<ActionResult<List<GameDto>>> Get([FromQuery] string? Name, [FromQuery] int? Page, [FromQuery] int? Count)
        {
            if (_GameContext == null)
            {
                return NotFound();
            }

            int page = Page == null ? 1 : (int)Page;
            int count = Count == null ? 16 : (int)Count;

            var games = await _GameContext.GetGamesAsync();

            if (Name != null)
            {
                games = games.Where(e => e.Name.Contains(Name)).ToList();
            }

            games = games.Skip((page - 1) * count);

            return Ok(games.ToList().ConvertAll(e=>e.AsDto()));
        }

        [HttpPost]
        public async Task<ActionResult> CreateGame([FromBody] CreateGameDto newGame)
        {
            if (_GameContext == null)
            {
                return NotFound();
            }

            var author = await _userManager.FindByIdAsync(User.FindFirstValue("id"));
            if (author == null)
            {
                return BadRequest();
            }

            Game game= new()
            {
                Genre = newGame.Genre,
                Name = newGame.Name,
                Price = newGame.Price,
                ReleaseDate = newGame.ReleaseDate,
                ApplicationUserId=author.Id,
                ApplicationUser=author
            };

            await _GameContext.CreateGameAsync(game);

            return CreatedAtRoute("getGameById", new { id = game.Id }, game.AsDto());
        }

        [HttpGet("{id}", Name = "getGameById")]
        public async Task<ActionResult<GameDto>> GetGameById(int id)
        {
            if (_GameContext == null)
            {
                return NotFound();
            }

            var game = await _GameContext.GetGameAsync(id);
            if (game == null)
                return NotFound();

            return Ok(game.AsDto());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DelteGameById(int id)
        {
            if (_GameContext == null)
            {
                return NotFound();
            }

            try
            {
                await _GameContext.DeleteGameAsync(id);
            }
            catch (Exception ex) { }


            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<GameDto>> UpdateGame(int id, [FromBody] UpdateGameDto game)
        {
            if (_GameContext == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid == false)
            {
                return BadRequest();
            }

            try
            {
                var existingGame = await _GameContext.GetGameAsync(id);

                if (existingGame == null)
                    return NoContent();

                existingGame.ReleaseDate = game.ReleaseDate;
                existingGame.Name = game.Name;
                existingGame.Price = game.Price;
                existingGame.Genre = game.Genre;

                await _GameContext.UpdateGameAsync(existingGame);

                return Ok(existingGame.AsDto());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
