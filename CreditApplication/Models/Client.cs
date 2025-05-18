using System.ComponentModel.DataAnnotations;

namespace CreditApplication.Models
{
    public class Client
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string? MiddleName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(10)]
        public string EGN { get; set; }

        [MaxLength(100)]
        public string? Email { get; set; }

        [MaxLength(10)]
        public string? PhoneNumber { get; set; }

        [Required]
        [MaxLength(20)]
        public string IDCardNumber { get; set; }

        [DataType(DataType.Date)]
        [Required]
        [Display(Name = "Дата на издаване")]
        public DateTime IDIssueDate { get; set; }

        [DataType(DataType.Date)]
        [Required]
        [Display(Name = "Дата на валидност")]
        public DateTime IDValidityDate { get; set; }

        [Required]
        [MaxLength(50)]
        public string IDIssuer { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime ModifiedOn { get; set; } = DateTime.Now;

        public ICollection<Credit> Credits { get; set; } = new List<Credit>();

        
    }
}
