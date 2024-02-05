using LookupAPI.Entities;
using LookupAPI.Contexts;
using Microsoft.EntityFrameworkCore;

namespace LookupAPI.Repositories.FilmRepos
{
    public class FilmRepository : IFilmRepository
    {
        private readonly LookupDbContextInMem _context;

        public FilmRepository(LookupDbContextInMem context)
        {
            this._context = context;
        }

        public async Task<int> CountAsync()
        {
            return await _context.Films.CountAsync();
        }

        public async Task CreateFilmAsync(Film film)
        {
            _context.Films.AddAsync(film);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFilmAsync(int id)
        {
            _context.Remove(await _context.Films.FirstAsync(film => film.Id == id));
            await _context.SaveChangesAsync();
        }

        public async Task<Film> GetFilmAsync(int filmId)
        {
            return await _context.Films.Include(c => c.ApplicationUser).FirstAsync(c=>c.Id==filmId);
        }

        public async Task<Film> GetFilmByNameAsync(string name)
        {
            return await _context.Films.Include(c =>c.ApplicationUser).FirstAsync(e => e.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        }

        public async Task<IEnumerable<Film>> GetFilmsAsync()
        {
            return await _context.Films.Include(c => c.ApplicationUser).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Film>> GetFilmsByCategoryAsync(string category)
        {
            return await _context.Films.Include(c => c.ApplicationUser).Where(e => e.Category.Equals(category, StringComparison.CurrentCultureIgnoreCase)).AsNoTracking().ToListAsync();
        }

        public async Task UpdateFilmAsync(Film film)
        {
            _context.Update(film);
            await _context.SaveChangesAsync();
        }
    }
}
