using LookupAPI.Entities;
using LookupAPI.Repositories.RecipeRepos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LookupAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private IRecipeRepository _RecipeContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public RecipeController(IRecipeRepository recipeContext, UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            _RecipeContext = recipeContext;
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<List<RecipeDto>>> Get() 
        {
            if (_RecipeContext == null)
            {
                return NotFound();
            }

            var recipes = await _RecipeContext.GetRecipesAsync();

            return Ok(recipes.ToList().ConvertAll(e=>e.AsDto()));
        }

        [HttpPost]
        public async Task<ActionResult> CreateRecipe([FromBody] CreateRecipeDto newRecipe)
        {
            if (_RecipeContext == null)
            {
                return NotFound();
            }

            var author = await _userManager.FindByIdAsync(User.FindFirstValue("id"));
            if (author == null)
            {
                return BadRequest();
            }

            Recipe recipe = new()
            {
               Ingredients=newRecipe.Ingredients.ToList(),
               Name=newRecipe.Name,
               RecipeDescription=newRecipe.RecipeDescription,
               RequiredTimeIntermsSeconds=newRecipe.RequiredTimeIntermsSeconds,
               ApplicationUserId=author.Id,
               ApplicationUser=author
            };

            await _RecipeContext.CreateRecipeAsync(recipe);

            return CreatedAtRoute("getRecipeById", new { id = recipe.Id }, recipe.AsDto());
        }

        [HttpGet("{id}", Name ="getRecipeById")]
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
        public async Task<ActionResult> DelteRecipeById(int id)
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
        public async Task<ActionResult<RecipeDto>> UpdateRecipe(int id, [FromBody] UpdateRecipeDto recipe)
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
