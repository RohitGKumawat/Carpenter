using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Carpenter.Models
{
    public class WorkProject
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ProjectName { get; set; }

        public string Description { get; set; }

        [Required]
        public bool IsCompleted { get; set; } = false; // Default: Not completed

        // Foreign key for UserProfile
        [ForeignKey("UserProfile")]
        public int UserProfileId { get; set; }

        public virtual UserProfile UserProfile { get; set; }
    }
}
