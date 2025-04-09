using System.ComponentModel.DataAnnotations;

namespace CreditApplication.Models
{
    public class Credit
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public int ClientID { get; set; }
        public Client Client { get; set; }
        [Required]
        public decimal CreditAmount { get; set; }
        public DateTime CreditBeginDate { get; set; } // Date when the credit begins
        public DateTime CreditEndDate { get; set; } // Date when the credit ends
        public decimal InterestRate { get; set; } // Interest rate for the credit
        public int Status { get; set; } // Status of the credit (e.g., active, closed, etc.)
        public Nomenclature StatusNomenclature { get; set; } // Foreign key to the Nomenclature table
        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        [Required]
        public DateTime ModifiedOn { get; set; } = DateTime.Now;
        [Required]
        public int CreditType { get; set; }
        public Nomenclature CreditTypeNomenclature { get; set; }
    }
}
