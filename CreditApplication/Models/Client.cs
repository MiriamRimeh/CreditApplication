using System.ComponentModel.DataAnnotations;

namespace CreditApplication.Models
{
    public class Client
    {
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

        [MaxLength(20)]
        public string? Email { get; set; }

        [MaxLength(10)]
        public string? PhoneNumber { get; set; }

        [Required]
        [MaxLength(20)]
        public string IDCardNumber { get; set; }

        [Required]
        public DateTime IDValidityDate { get; set; }

        [Required]
        public DateTime IDIssueDate { get; set; }

        [Required]
        [MaxLength(50)]
        public string IDIssuer { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime ModifiedOn { get; set; } = DateTime.Now;
    }
}
