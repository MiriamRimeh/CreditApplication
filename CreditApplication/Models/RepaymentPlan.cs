using System.ComponentModel.DataAnnotations;

namespace CreditApplication.Models
{
    public class RepaymentPlan
    {
        [Key]
        public int ID { get; set; }
        public int CreditID { get; set; }
        public Credit Credit { get; set; }

        public int? InstallmentNumber { get; set; } // Number of the installment in the repayment plan

        public DateOnly? InstallmentDate { get; set; } // Date when the installment is due
        public decimal? InstallmentAmount { get; set; } // Amount to be paid in each installment (total)
        public decimal? Principal { get; set; } // Amount of the principal in the installment from the total
        public decimal? Interest { get; set; } // Amount of the interest in the installment from the total

        public bool? isPaid { get; set; } = false; // Indicates if the installment has been paid
        public DateTime? PayedOnDate { get; set; } // Date when the installment is due
        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        [Required]
        public DateTime ModifiedOn { get; set; } = DateTime.Now;
    }
}