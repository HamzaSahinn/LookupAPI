using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;
using System.Collections.ObjectModel;

namespace LookupAPI.Entities
{
    public class ApplicationUser: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<Recipe> Recipes { get; set; }

        public ICollection<Film> Films { get; set; }

        public ICollection<Game> Games { get; set; }
    }
}
