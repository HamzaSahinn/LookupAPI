using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LookupAPI.Entities
{
    public class Film
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string? Name { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        [Required]
        [Range(1, Int32.MaxValue - 1)]
        public int LengthInSeconds { get; set; }

        [Required]
        [StringLength(25)]
        public string? Category { get; set; }
        
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}
