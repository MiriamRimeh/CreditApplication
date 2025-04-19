using System;
using System.Collections.Generic;

namespace FastCreditApp.Data.Entities;

public partial class RepaymentPlan
{
    public int Id { get; set; }

    public int CreditId { get; set; }

    public int? InstallmentNumber { get; set; }

    public DateOnly? InstallmentDate { get; set; }

    public decimal? InstallmentAmount { get; set; }

    public decimal? Principal { get; set; }

    public decimal? Interest { get; set; }

    public bool? IsPaid { get; set; }

    public DateOnly? PayedOnDate { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime ModifiedOn { get; set; }

    public virtual Credit Credit { get; set; } = null!;
}
