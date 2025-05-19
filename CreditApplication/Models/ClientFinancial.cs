using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CreditApplication.Models
{
    public class ClientFinancial
    {
        public int ID { get; set; }

        [TempData]
        public int ClientID { get; set; }
        [BindProperty]
        public Client Client { get; set; }

        [Required(ErrorMessage = "Моля, въведете размер на доходи.")]
        public decimal MontlyIncome { get; set; }

        [Required(ErrorMessage = "Моля, въведете размер на разходи.")]
        public decimal MontlyExpenses { get; set; }

        [Required(ErrorMessage = "Моля, въведете вид заетост.")]
        public int EmploymentType { get; set; }

        [ForeignKey("EmploymentType")]
        public Nomenclature EmploymentTypeNomenclature { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime ModifiedOn { get; set; } = DateTime.Now;
    }
}