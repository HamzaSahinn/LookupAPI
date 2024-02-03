using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LookupAPI.Entities
{
    public class Recipe
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string? Name { get; set; }

        public List<string>? Ingredients { get; set; }

        [Required]
        [Range(60, Int32.MaxValue - 1)]
        public int RequiredTimeIntermsSeconds { get; set; }

        [Required]
        [StringLength(500)]
        public string? RecipeDescription {  get; set; }

        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
