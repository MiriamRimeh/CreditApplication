using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace CreditApplication.Models
{
    public class Users
    {
        public int ID { get; set; }

        public string Username { get; set; }

        public int? ClientID { get; set; }

        [BindProperty]
        [ForeignKey(nameof(ClientID))]
        public virtual Client Client { get; set; }

        [Column(TypeName = "varbinary(64)")]
        public byte[] PasswordHash { get; set; }

        [Column(TypeName = "varbinary(128)")]
        public byte[] PasswordSalt { get; set; }

        public byte UserType { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime ModifiedOn21180011 { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;

    }
}
