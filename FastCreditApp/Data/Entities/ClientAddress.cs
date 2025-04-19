using System;
using System.Collections.Generic;

namespace FastCreditApp.Data.Entities;

public partial class ClientAddress
{
    public int ID { get; set; }

    public string City { get; set; } = null!;

    public string StreetNeighbourhood { get; set; } = null!;

    public string Number { get; set; } = null!;

    public string PostCode { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public DateTime ModifiedOn { get; set; }

    public int? ClientID { get; set; }

    public virtual Client? Client { get; set; }
}
