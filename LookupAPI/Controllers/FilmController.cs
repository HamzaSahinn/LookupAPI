using LookupAPI.Entities;
using LookupAPI.Repositories.FilmRepos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LookupAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmController : ControllerBase
    {
        private readonly IFilmRepository _FilmContext;

        public FilmController(IFilmRepository filmContext)
        {
            _FilmContext = filmContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FilmDto>>> Get([FromQuery] string? Name)
        {
            if(_FilmContext == null)
            {
                return NotFound();
            }

            var films = await _FilmContext.GetFilmsAsync();

            if(Name != null)
            {
                films = films.Where(e => e.Name.Contains(Name)).ToList();
            }

            return Ok(films);
        }

        [HttpPost]
        public async Task<ActionResult> CreateFilm([FromBody] CreateFilmDto newFilm)
        {
            if (_FilmContext == null)
            {
                return NotFound();
            }

            Film film = new() { 
                LengthInSeconds = newFilm.LengthInSeconds,
                Name = newFilm.Name,
                Category = newFilm.Category,
                ReleaseDate = newFilm.ReleaseDate,
            };

            await _FilmContext.CreateFilmAsync(film);

            return Created();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Film>> GetFilmById(int id)
        {
            if (_FilmContext == null)
            {
                return NotFound();
            }

            var film = await _FilmContext.GetFilmAsync(id);
            if(film == null)
                return NotFound();

            return Ok(film.AsDto());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DelteFilmById(int id)
        {
            if (_FilmContext == null)
            {
                return NotFound();
            }

            try
            {
                await _FilmContext.DeleteFilmAsync(id);
            }
            catch (Exception ex) { }


            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Film>> UpdateFilm(int id, [FromBody] UpdateFilmDto film)
        {
            if (_FilmContext == null)
            {
                return NotFound();
            }

            if(ModelState.IsValid == false) 
            {
                return BadRequest();
            }

            try
            {
                var existingFilm = await _FilmContext.GetFilmAsync(id);

                if (existingFilm == null)
                    return NoContent();

                existingFilm.LengthInSeconds = film.LengthInSeconds;
                existingFilm.ReleaseDate = film.ReleaseDate;
                existingFilm.Category = film.Category;
                existingFilm.Name = film.Name;

                await _FilmContext.UpdateFilmAsync(existingFilm);

                return Ok(existingFilm.AsDto());
            }
            catch (Exception ex)
            { 
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
