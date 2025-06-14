using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Carpenter.Models
{
    public class ProjectItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ItemName { get; set; }

        public string Description { get; set; }

        public decimal Height_inches { get; set; }
        public decimal Width_inches { get; set; }
        public decimal Length_inches { get; set; }

        public decimal Height_mm { get; set; }
        public decimal Width_mm { get; set; }
        public decimal Length_mm { get; set; }

        // Foreign key to project
        [ForeignKey("WorkProject")]
        public int WorkProjectId { get; set; }

        [ValidateNever]
        public virtual WorkProject WorkProject { get; set; }
    }
}
