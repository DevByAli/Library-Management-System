using LMS.Models;
using System;
using System.Collections.Generic;

namespace LMS;

public partial class IssuedBook 
{
    public string UserId { get; set; } = null!;

    public string Iban { get; set; } = null!;

    public DateTime IssuedDate { get; set; }
}
