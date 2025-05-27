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

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [Column("ModifiedOn_21180011")]
        public DateTime ModifiedOn { get; set; } = DateTime.Now;
    }
}