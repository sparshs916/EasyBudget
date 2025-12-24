using System.ComponentModel.DataAnnotations;

namespace EasyBudget.Api.Models;

public class User
{   
    [Key]
    public Guid Guid { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(32)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Auth0Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}