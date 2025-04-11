using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CreditApplication.Models
{
    public class ClientAddress
    {
        [Key]
        public int ID { get; set; }

        [TempData]
        public int ClientID { get; set; }
        [BindProperty]
        public Client Client { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string StreetNeighbourhood { get; set; }

        [MaxLength(10)]
        public string? Number { get; set; }
        [Required]
        [MaxLength(20)]
        public string PostalCode { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime ModifiedOn { get; set; } = DateTime.Now;
    }
}