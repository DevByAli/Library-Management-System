using System;
using System.Collections.Generic;

namespace LMS;

public partial class RequestBook
{
    public string UserId { get; set; } = null!;

    public string Iban { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
