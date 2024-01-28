using LookupAPI.Entities;
using LookupAPI.Repositories.GameRepos;
using LookupAPI.Repositories.RecipeRepos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LookupAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private IGameRepository _GameContext;

        public GameController(IGameRepository gameRepository)
        {
            _GameContext = gameRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<GameDto>>> Get()
        {
            if (_GameContext == null)
            {
                return NotFound();
            }

            var games = await _GameContext.GetGamesAsync();

            return Ok(games);
        }

        [HttpPost]
        public async Task<ActionResult> CreateGame([FromBody] CreateGameDto newGame)
        {
            if (_GameContext == null)
            {
                return NotFound();
            }

            Game game= new()
            {
                Genre = newGame.Genre,
                Name = newGame.Name,
                Price = newGame.Price,
                ReleaseDate = newGame.ReleaseDate,
            };

            await _GameContext.CreateGameAsync(game);

            return Created();
        }

        [HttpGet("{id}")]
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
