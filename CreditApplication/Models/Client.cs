using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CreditApplication.Models
{
    public class Client
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage ="Моля, въведете име.")]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "Моля, въведете фамилия.")]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Моля, въведете ЕГН.")]
        [MaxLength(10)]
        public string EGN { get; set; }

        [MaxLength(100), EmailAddress]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Моля, въведете телефонен номер.")]
        [MaxLength(10)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Моля, въведете № на лична карта.")]
        [MaxLength(20)]
        public string IDCardNumber { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Моля, въведете дата на издаване на лична карта.")]
        [Display(Name = "Дата на издаване")]
        public DateTime IDIssueDate { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Моля, въведете дата на валидност на лична карта.")]
        [Display(Name = "Дата на валидност")]
        public DateTime IDValidityDate { get; set; }

        [Required(ErrorMessage = "Моля, въведете място на издаване на лична карта.")]
        [MaxLength(50)]
        public string IDIssuer { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [Column("ModifiedOn_21180011")]
        public DateTime ModifiedOn { get; set; } = DateTime.Now;

        public ICollection<Credit> Credits { get; set; }
        public ICollection<ClientFinancial> ClientFinancials { get; set; }
        public ICollection<ClientAddress> ClientAddresses { get; set; }
        public ICollection<Account> Accounts { get; set; }


    }
}
