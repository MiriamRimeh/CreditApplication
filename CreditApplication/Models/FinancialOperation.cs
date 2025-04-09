using System.ComponentModel.DataAnnotations;

namespace CreditApplication.Models
{
    public class FinancialOperation
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public int CreditID { get; set; }
        public Credit Credit { get; set; }
        public DateTime PayedOnDate { get; set; }
        public decimal PayedAmount { get; set; }

        public int OperationType { get; set; } 
        public Nomenclature OperationTypeNomenclature { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        [Required]
        public DateTime ModifiedOn { get; set; } = DateTime.Now;
    }
}
