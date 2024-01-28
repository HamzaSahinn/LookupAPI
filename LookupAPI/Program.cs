using LookupAPI.Contexts;
using LookupAPI.Repositories.FilmRepos;
using LookupAPI.Repositories.RecipeRepos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<LookupDbContextInMem>();

builder.Services.AddScoped<IFilmRepository, FilmRepository>();
builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
