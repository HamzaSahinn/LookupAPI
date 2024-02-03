using LookupAPI.Contexts;
using LookupAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace LookupAPI.Repositories.GameRepos
{
    public class GameRepository : IGameRepository
    {
        private readonly LookupDbContextInMem _context;

        public GameRepository(LookupDbContextInMem context)
        {
            _context = context;
        }

        public async Task CreateGameAsync(Game game)
        {
            _context.Games.AddAsync(game);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteGameAsync(int id)
        {
            _context.Remove(await _context.Games.FirstAsync(game => game.Id == id));
            await _context.SaveChangesAsync();
        }

        public async Task<Game> GetGameAsync(int gameId)
        {
            return await _context.Games.Include(e=>e.ApplicationUser).FirstAsync(e=>e.Id == gameId);
        }

        public async Task<IEnumerable<Game>> GetGamesAsync()
        {
            return await _context.Games.Include(e => e.ApplicationUser).AsNoTracking().ToListAsync();
        }

        public async Task UpdateGameAsync(Game game)
        {
            _context.Update(game);
            await _context.SaveChangesAsync();
        }
    }
}
