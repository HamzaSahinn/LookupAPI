﻿namespace LookupAPI.Entities
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
                film.Category
                );
        }

        public static RecipeDto AsDto(this Recipe recipe)
        {
            return new RecipeDto(
                recipe.Id,
                recipe.Name,
                recipe.Ingredients.ToArray(),
                recipe.RequiredTimeIntermsSeconds,
                recipe.RecipeDescription
                );
        }
    }
}