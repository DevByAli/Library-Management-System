using System.ComponentModel.DataAnnotations;

namespace LMS;

public partial class User
{
    public string Id { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool? Admin { get; set; }

    public virtual RequestBook? RequestBook { get; set; }
}
