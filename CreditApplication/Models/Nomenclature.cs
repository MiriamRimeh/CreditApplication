using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CreditApplication.Models
{
    public class Nomenclature
    {
        [Key]
        public int NomCode { get; set; }

        [Required]
        [MaxLength(255)]
        public string Description { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime ModifiedOn { get; set; } = DateTime.Now;
    }
}
