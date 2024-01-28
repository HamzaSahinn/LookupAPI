using LookupAPI.Entities;

namespace LookupAPI.Repositories.FilmRepos
{
    public interface IFilmRepository
    {
        public Task<IEnumerable<Film>> GetFilmsAsync();

        public Task<Film> GetFilmAsync(int filmId);

        public Task<Film> GetFilmByNameAsync(string name);

        public Task<IEnumerable<Film>> GetFilmsByCategoryAsync(string category);

        public Task UpdateFilmAsync(Film film);

        public Task DeleteFilmAsync(int id);

        public Task CreateFilmAsync(Film film);
    }
}
