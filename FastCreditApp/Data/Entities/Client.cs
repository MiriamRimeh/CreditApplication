using System;
using System.Collections.Generic;

namespace FastCreditApp.Data.Entities;

public partial class Client
{
    public int ID { get; set; }

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string LastName { get; set; } = null!;

    public string EGN { get; set; } = null!;

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string IDCardNumber { get; set; } = null!;

    public DateOnly IDValidityDate { get; set; }

    public DateOnly IDIssueDate { get; set; }

    public string IDIssuer { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public DateTime ModifiedOn { get; set; }

    public virtual ICollection<ClientAddress> ClientAddresses { get; set; } = new List<ClientAddress>();

    public virtual ICollection<ClientFinancial> ClientFinancials { get; set; } = new List<ClientFinancial>();

    public virtual ICollection<Credit> Credits { get; set; } = new List<Credit>();
}
