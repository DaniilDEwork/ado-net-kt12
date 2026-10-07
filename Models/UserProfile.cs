using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kt12Profiles.Models;

public class UserProfile
{
    [Key]
    [ForeignKey(nameof(User))]
    public int UserId { get; set; }

    [Required]
    [StringLength(1000)]
    public string Bio { get; set; } = "";

    [StringLength(100)]
    public string City { get; set; } = "";

    public User User { get; set; } = null!;
}
