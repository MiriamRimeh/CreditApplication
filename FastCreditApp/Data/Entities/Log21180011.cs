using System;
using System.Collections.Generic;

namespace FastCreditApp.Data.Entities;

public partial class Log21180011
{
    public int Id { get; set; }

    public string? TableName { get; set; }

    public string? ActionType { get; set; }

    public DateTime? ActionDate { get; set; }
}
