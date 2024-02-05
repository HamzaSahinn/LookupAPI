using LookupAPI.Contexts;
using LookupAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace LookupAPI.Repositories.RecipeRepos
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly LookupDbContextInMem _context;

        public RecipeRepository(LookupDbContextInMem context)
        {
            this._context = context;
        }

        public async Task<int> CountAsync()
        {
            return await _context.Recipes.CountAsync();

        }

        public async Task CreateRecipeAsync(Recipe recipe)
        {
            _context.Recipes.AddAsync(recipe);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRecipeAsync(int id)
        {
            _context.Remove(await _context.Recipes.FirstAsync(recipe => recipe.Id == id));
            await _context.SaveChangesAsync();
        }

        public async Task<Recipe> GetRecipeAsync(int recipeId)
        {
            return await _context.Recipes.Include(e => e.ApplicationUser).FirstAsync(e=> e.Id == recipeId);
        }

        public async Task<IEnumerable<Recipe>> GetRecipesAsync()
        {
            return await _context.Recipes.Include(e => e.ApplicationUser).AsNoTracking().ToListAsync();
        }

        public async Task UpdateRecipeAsync(Recipe recipe)
        {
            _context.Update(recipe);
            await _context.SaveChangesAsync();
        }
    }
}
