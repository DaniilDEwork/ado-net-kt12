using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Kt12Profiles.Models;

[Index(nameof(Email), IsUnique = true)]
public class User
{
    [Key]
    public int UserId { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = "";

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = "";

    public UserProfile Profile { get; set; } = null!;
}
