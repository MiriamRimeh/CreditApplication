using System;
using System.Collections.Generic;

namespace FastCreditApp.Data.Entities;

public partial class FinancialOperation
{
    public int Id { get; set; }

    public int CreditId { get; set; }

    public DateOnly? PayedOnDate { get; set; }

    public decimal? PayedAmount { get; set; }

    public int? OperationType { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime ModifiedOn { get; set; }

    public virtual Credit Credit { get; set; } = null!;

    public virtual Nomenclature? OperationTypeNavigation { get; set; }
}
