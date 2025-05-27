using CreditApplication.Data;
using DocumentFormat.OpenXml.Wordprocessing;
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

        [ForeignKey(nameof(Client))]
        public int? ClientID { get; set; }
        public Client Client { get; set; }

        [Required]
        public byte[] PasswordHash { get; set; }

        [Required]
        public byte[] PasswordSalt { get; set; }

        [Required]
        public AccountRole Role { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }

        [Column("ModifiedOn_21180011")]
        public DateTime? ModifiedOn21180011 { get; set; } = DateTime.Now;
        public bool IsActive { get; set; }



    }
}
