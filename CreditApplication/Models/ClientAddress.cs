﻿using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CreditApplication.Models
{
    public class ClientAddress
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey(nameof(Client))]
        public int ClientID { get; set; }

        [BindProperty]
        public Client Client { get; set; }

        [Required(ErrorMessage = "Моля, въведете населено място.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Моля, въведете улица/жилищен комплекс.")]
        public string StreetNeighbourhood { get; set; }

        [MaxLength(10)]
        public string? Number { get; set; }

        [Required(ErrorMessage = "Моля, въведете пощенски код.")]
        [MaxLength(4)]
        public string PostCode { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [Column("ModifiedOn_21180011")]
        public DateTime ModifiedOn { get; set; } = DateTime.Now;
    }
}