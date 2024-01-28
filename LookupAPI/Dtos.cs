using System.ComponentModel.DataAnnotations;

namespace LookupAPI
{
    public record FilmDto(
        int id,
        string Name,
        DateTime ReleaseDate,
        int LengthInSeconds,
        string Category
        );

    public record CreateFilmDto(
        [Required][StringLength(50)] string Name,
        [Required] DateTime ReleaseDate,
        [Required][Range(1, Int32.MaxValue - 1)] int LengthInSeconds,
        [Required][StringLength(25)] string Category
        );

    public record UpdateFilmDto(
        [Required][StringLength(50)] string Name,
        [Required] DateTime ReleaseDate,
        [Required][Range(1, Int32.MaxValue - 1)] int LengthInSeconds,
        [Required][StringLength(25)] string Category
        );

    public record RecipeDto(
        int Id,
        string Name,
        string[] Ingredients,
        int RequiredTimeIntermsSeconds,
        string RecipeDescription
        );

    public record CreateRecipeDto(
        [Required][StringLength(50)] string Name,
        string[] Ingredients,
        [Required][Range(60, Int32.MaxValue - 1)] int RequiredTimeIntermsSeconds,
        [Required][StringLength(500)] string RecipeDescription
        );

    public record UpdateRecipeDto(
        [Required][StringLength(50)] string Name,
        string[] Ingredients,
        [Required][Range(60, Int32.MaxValue - 1)] int RequiredTimeIntermsSeconds,
        [Required][StringLength(500)] string RecipeDescription
        );

}
