using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CreditApplication.Models
{
    public class Credit
    {
        [Key]
        public int ID { get; set; }
        [TempData]
        public int ClientID { get; set; }
        [BindProperty]
        public Client Client { get; set; }
        [Required]
        public decimal CreditAmount { get; set; }
        public DateTime CreditBeginDate { get; set; } // Date when the credit begins
        public DateTime CreditEndDate { get; set; } // Date when the credit ends
        public decimal InterestRate { get; set; } // Interest rate for the credit
        public int Status { get; set; } // Status of the credit (e.g., active, closed, etc.)

        [ForeignKey("Status")]
        public Nomenclature StatusNavigation { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        [Required]
        public DateTime ModifiedOn { get; set; } = DateTime.Now;

        public ICollection<FinancialOperation> FinancialOperations { get; set; } = new List<FinancialOperation>();

    }
}