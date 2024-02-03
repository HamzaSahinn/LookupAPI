using System.ComponentModel.DataAnnotations;

namespace LookupAPI
{
    public record FilmDto(
        int id,
        string Name,
        DateTime ReleaseDate,
        int LengthInSeconds,
        string Category,
        string ApplicationUserId,
        string ApplicationUserName
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
        [Required][StringLength(25)] string Category,
        [Required] string ApplicationUserId
        );

    public record RecipeDto(
        int Id,
        string Name,
        string[] Ingredients,
        int RequiredTimeIntermsSeconds,
        string RecipeDescription,
        string ApplicationUserId,
        string ApplicationUserName
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



    public record GameDto(
        int Id,
        string Name,
        string Genre,
        decimal Price,
        DateTime ReleaseDate,
        string ApplicationUserId,
        string ApplicationUserName
    );

    public record CreateGameDto(
        [Required][StringLength(50)] string Name,
        [Required][StringLength(20)] string Genre,
        [Range(1, 500)] decimal Price,
        DateTime ReleaseDate
    );

    public record UpdateGameDto(
        [Required][StringLength(50)] string Name,
        [Required][StringLength(20)] string Genre,
        [Range(1, 500)] decimal Price,
        DateTime ReleaseDate
    );

    public record LoginDto(
        [Required]string? Email,
        [Required]string? Password
        );

    public record RegisterDto(
        [Required]string? Email,
        [Required]string? Password,
        [Required][StringLength(100)] string? FirstName,
        [Required][StringLength(100)] string? LastName
        );
}
