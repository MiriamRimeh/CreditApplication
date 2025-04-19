using System;
using System.Collections.Generic;

namespace FastCreditApp.Data.Entities;

public partial class ClientFinancial
{
    public int ID { get; set; }

    public int ClientID { get; set; }

    public decimal MontlyIncome { get; set; }

    public decimal MontlyExpenses { get; set; }

    public int EmploymentType { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime ModifiedOn { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual Nomenclature EmploymentTypeNavigation { get; set; } = null!;
}
