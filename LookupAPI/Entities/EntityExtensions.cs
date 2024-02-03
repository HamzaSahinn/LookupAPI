namespace LookupAPI.Entities
{
    public static class EntityExtensions
    {
        public static FilmDto AsDto(this Film film)
        {
            return new FilmDto(
                film.Id,
                film.Name,
                film.ReleaseDate,
                film.LengthInSeconds,
                film.Category,
                film.ApplicationUserId,
                film.ApplicationUser.FirstName + " " + film.ApplicationUser.LastName
                );
        }

        public static RecipeDto AsDto(this Recipe recipe)
        {
            return new RecipeDto(
                recipe.Id,
                recipe.Name,
                recipe.Ingredients.ToArray(),
                recipe.RequiredTimeIntermsSeconds,
                recipe.RecipeDescription,
                recipe.ApplicationUserId,
                recipe.ApplicationUser.FirstName + " " + recipe.ApplicationUser.LastName
                );
        }

        public static GameDto AsDto(this Game game)
        {
            return new GameDto(
                game.Id,
                game.Name,
                game.Genre,
                game.Price,
                game.ReleaseDate,
                game.ApplicationUserId,
                game.ApplicationUser.FirstName + " " + game.ApplicationUser.LastName
            );
        }
    }
}
