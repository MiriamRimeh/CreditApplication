using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FastCreditApp.Data.Entities;

public partial class Client
{
    public int ID { get; set; }

    [Required]
    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    [Required]
    public string LastName { get; set; } = null!;

    [Required]
    [StringLength(10, MinimumLength = 10, ErrorMessage = "Дължината трябва да е 10 символа")]
    public string EGN { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Phone]
    public string? PhoneNumber { get; set; }

    [Required]
    public string IDCardNumber { get; set; } = null!;

    [Required]
    public DateOnly IDValidityDate { get; set; }

    [Required]
    public DateOnly IDIssueDate { get; set; }

    [Required]
    public string IDIssuer { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public DateTime ModifiedOn { get; set; }

    public virtual ICollection<ClientAddress> ClientAddresses { get; set; } = new List<ClientAddress>();

    public virtual ICollection<ClientFinancial> ClientFinancials { get; set; } = new List<ClientFinancial>();

    public virtual ICollection<Credit> Credits { get; set; } = new List<Credit>();
}
