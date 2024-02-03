using LookupAPI.Contexts;
using LookupAPI.Entities;
using LookupAPI.Repositories.FilmRepos;
using LookupAPI.Repositories.GameRepos;
using LookupAPI.Repositories.RecipeRepos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("policy",
                          policy =>
                          {
                              policy.WithOrigins("http://localhost:3000")
                                                  .AllowAnyHeader()
                                                  .AllowAnyMethod();
                          });
});

builder.Services.AddDbContext<LookupDbContextInMem>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<LookupDbContextInMem>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

// Adding Jwt Bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };
});

builder.Services.AddScoped<IFilmRepository, FilmRepository>();
builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();
builder.Services.AddScoped<IGameRepository, GameRepository>();

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseCors("policy");

app.UseAuthorization();

app.MapControllers();

app.Run();
