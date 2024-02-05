using LookupAPI.Entities;

namespace LookupAPI.Repositories.RecipeRepos
{
    public interface IRecipeRepository
    {
        public Task<IEnumerable<Recipe>> GetRecipesAsync();

        public Task<Recipe> GetRecipeAsync(int recipeId);

        public Task UpdateRecipeAsync(Recipe recipe);

        public Task DeleteRecipeAsync(int id);

        public Task CreateRecipeAsync(Recipe recipe);

        public Task<int> CountAsync();

    }
}
