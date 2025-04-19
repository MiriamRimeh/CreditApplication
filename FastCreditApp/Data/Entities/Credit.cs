using System;
using System.Collections.Generic;

namespace FastCreditApp.Data.Entities;

public partial class Credit
{
    public int ID { get; set; }

    public int ClientID { get; set; }

    public decimal CreditAmount { get; set; }

    public DateOnly? CreditBeginDate { get; set; }

    public DateOnly? CreditEndDate { get; set; }

    public int? Status { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime ModifiedOn { get; set; }

    public decimal? InterestRate { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual ICollection<FinancialOperation> FinancialOperations { get; set; } = new List<FinancialOperation>();

    public virtual ICollection<RepaymentPlan> RepaymentPlans { get; set; } = new List<RepaymentPlan>();

    public virtual Nomenclature? StatusNavigation { get; set; }
}
