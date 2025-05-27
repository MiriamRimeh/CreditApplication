using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CreditApplication.Models
{
    public class Credit
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey(nameof(Client))]
        public int ClientID { get; set; }
        [BindProperty]
        public Client Client { get; set; }

        [Required(ErrorMessage = "Моля, въведете желаната сума.")]
        [Range(300, 5000, ErrorMessage = "Сумата трябва да е между 300 и 5000 лв.")]
        public decimal CreditAmount { get; set; }

        public DateTime? CreditBeginDate { get; set; } 
        public DateTime? CreditEndDate { get; set; } 
        public decimal? InterestRate { get; set; }

        [ForeignKey(nameof(Nomenclature))]
        public int Status { get; set; } 

        [ForeignKey("Status")]
        public Nomenclature StatusNavigation { get; set; }


        [Required(ErrorMessage = "Моля, въведете периода на кредита.")]
        [Range(5, 24, ErrorMessage = "Периодът трябва да бъде между 5 и 24 месеца.")]
        public int? CreditPeriod { get; set; }
        public decimal? MonthlyInstallment { get; set; }
        public decimal? TotalCreditAmount { get; set; }


        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime? CreatedOn { get; set; }

        [Required]
        [Column("ModifiedOn_21180011")]
        public DateTime? ModifiedOn { get; set; } = DateTime.Now;
        

        public DateOnly? ActivationDate { get; set; }

        public ICollection<FinancialOperation> FinancialOperations { get; set; } = new List<FinancialOperation>();
        public ICollection<RepaymentPlan> RepaymentPlans { get; set; }




    }
}