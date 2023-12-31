using LMS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LMS;

public partial class Book : Audit
{
    [Required(ErrorMessage = "Enter IBAN")]
    public string Iban { get; set; } = null!;

    [Required(ErrorMessage = "Enter Name")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Enter Author")]
    public string Author { get; set; } = null!;
}
