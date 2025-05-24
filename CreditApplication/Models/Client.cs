using System.ComponentModel.DataAnnotations;

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

        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime ModifiedOn { get; set; } = DateTime.Now;

        public ICollection<Credit> Credits { get; set; } = new List<Credit>();

        public ClientAddress? ClientAddress { get; set; }
        public ClientFinancial? ClientFinancial { get; set; }

        //public ICollection<ClientAddress> Addresses { get; set; }


    }
}
