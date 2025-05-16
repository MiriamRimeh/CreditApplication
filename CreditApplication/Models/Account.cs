using CreditApplication.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CreditApplication.Models
{
    public class Account
    {
        public int ID { get; set; }

        [Required, EmailAddress, MaxLength(256)]
        public string Username { get; set; }

        public int? ClientID { get; set; }

        [Required]
        public byte[] PasswordHash { get; set; }

        [Required]
        public byte[] PasswordSalt { get; set; }

        [Required]
        public AccountRole Role { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedOn21180011 { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey(nameof(ClientID))]
        public virtual Client Client { get; set; }
    }
}
