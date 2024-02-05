using LookupAPI.Entities;
using LookupAPI.Repositories.FilmRepos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace LookupAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmController : ControllerBase
    {
        private readonly IFilmRepository _FilmContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public FilmController(IFilmRepository filmContext, UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            _FilmContext = filmContext;
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetCount()
        {
            if (_FilmContext == null)
            {
                return NotFound();
            }

            return Ok(await _FilmContext.CountAsync());
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<FilmDto>>> Get([FromQuery] string? Name, [FromQuery] int? Page, [FromQuery] int? Count)
        {
            if (_FilmContext == null)
            {
                return NotFound();
            }


            int page = Page == null ? 1 : (int)Page;
            int count  = Count == null ? 16 : (int)Count;

            var films = await _FilmContext.GetFilmsAsync();


            if (Name != null)
            {
                films = films.Where(e => e.Name.Contains(Name)).ToList();
            }

            films = films.Skip((page - 1) * count);

            return Ok(films.ToList().ConvertAll(e => e.AsDto()));
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateFilm([FromBody] CreateFilmDto newFilm)
        {
            if (_FilmContext == null)
            {
                return NotFound();
            }

            var author = await _userManager.FindByIdAsync(User.FindFirstValue("id"));
            if(author == null)
            {
                return BadRequest();
            }

            Film film = new() { 
                LengthInSeconds = newFilm.LengthInSeconds,
                Name = newFilm.Name,
                Category = newFilm.Category,
                ReleaseDate = newFilm.ReleaseDate,
                ApplicationUserId = author.Id,
                ApplicationUser = author,
            };

            await _FilmContext.CreateFilmAsync(film);

            return CreatedAtRoute("getFilmById", new { id = film.Id }, film.AsDto());
        }

        [HttpGet("{id}", Name = "getFilmById")]
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

        [Authorize]
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

        [Authorize]
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
