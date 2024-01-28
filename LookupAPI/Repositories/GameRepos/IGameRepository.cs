using LookupAPI.Entities;

namespace LookupAPI.Repositories.GameRepos
{
    public interface IGameRepository
    {
        public Task<IEnumerable<Game>> GetGamesAsync();

        public Task<Game> GetGameAsync(int gameId);

        public Task UpdateGameAsync(Game game);

        public Task DeleteGameAsync(int id);

        public Task CreateGameAsync(Game game);
    }
}
