using LookupAPI.Entities;
using LookupAPI.Repositories.RecipeRepos;
using Microsoft.AspNetCore.Mvc;

namespace LookupAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private IRecipeRepository _RecipeContext;

        public RecipeController(IRecipeRepository recipeContext)
        {
            _RecipeContext = recipeContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<RecipeDto>>> Get() 
        {
            if (_RecipeContext == null)
            {
                return NotFound();
            }

            var recipes = await _RecipeContext.GetRecipesAsync();

            return Ok(recipes);
        }

        [HttpPost]
        public async Task<ActionResult> CreateRecipe([FromBody] CreateRecipeDto newRecipe)
        {
            if (_RecipeContext == null)
            {
                return NotFound();
            }

            Recipe recipe = new()
            {
               Ingredients=newRecipe.Ingredients.ToList(),
               Name=newRecipe.Name,
               RecipeDescription=newRecipe.RecipeDescription,
               RequiredTimeIntermsSeconds=newRecipe.RequiredTimeIntermsSeconds,
            };

            await _RecipeContext.CreateRecipeAsync(recipe);

            return Created();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RecipeDto>> GetRecipeById(int id)
        {
            if (_RecipeContext == null)
            {
                return NotFound();
            }

            var recipe = await _RecipeContext.GetRecipeAsync(id);
            if (recipe == null)
                return NotFound();

            return Ok(recipe.AsDto());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DelteFilmById(int id)
        {
            if (_RecipeContext == null)
            {
                return NotFound();
            }

            try
            {
                await _RecipeContext.DeleteRecipeAsync(id);
            }
            catch (Exception ex) { }


            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<RecipeDto>> UpdateFilm(int id, [FromBody] UpdateRecipeDto recipe)
        {
            if (_RecipeContext == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid == false)
            {
                return BadRequest();
            }

            try
            {
                var existingRecipe = await _RecipeContext.GetRecipeAsync(id);

                if (existingRecipe == null)
                    return NoContent();

                existingRecipe.RecipeDescription=recipe.RecipeDescription;
                existingRecipe.RequiredTimeIntermsSeconds = recipe.RequiredTimeIntermsSeconds;
                existingRecipe.Ingredients = recipe.Ingredients.ToList();
                existingRecipe.Name = recipe.Name;

                await _RecipeContext.UpdateRecipeAsync(existingRecipe);

                return Ok(existingRecipe.AsDto());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
