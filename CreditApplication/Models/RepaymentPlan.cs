using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CreditApplication.Models
{
    public class RepaymentPlan
    {
        [Key]
        public int ID { get; set; }
        public int CreditID { get; set; }
        public Credit Credit { get; set; }

        public int? InstallmentNumber { get; set; } 

        public DateOnly? InstallmentDate { get; set; } 
        public decimal? InstallmentAmount { get; set; } 
        public decimal? Principal { get; set; } 
        public decimal? Interest { get; set; } 

        public bool? isPaid { get; set; } = false; 
        public DateTime? PayedOnDate { get; set; } 

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [Required]
        [Column("ModifiedOn_21180011")]
        public DateTime ModifiedOn { get; set; } = DateTime.Now;
    }
}