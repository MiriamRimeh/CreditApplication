using System;
using System.Collections.Generic;

namespace FastCreditApp.Data.Entities;

public partial class Nomenclature
{
    public int NomCode { get; set; }

    public string Description { get; set; } = null!;

    public DateTime? CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public virtual ICollection<ClientFinancial> ClientFinancials { get; set; } = new List<ClientFinancial>();

    public virtual ICollection<Credit> Credits { get; set; } = new List<Credit>();

    public virtual ICollection<FinancialOperation> FinancialOperations { get; set; } = new List<FinancialOperation>();
}
