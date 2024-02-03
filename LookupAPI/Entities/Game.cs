using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LookupAPI.Entities
{
    public class Game
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public required string Name { get; set; }

        [Required]
        [StringLength(20)]
        public required string Genre { get; set; }

        [Range(1, 500)]
        public decimal Price { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}
