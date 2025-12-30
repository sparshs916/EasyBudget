
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EasyBudget.Api.Models;

public class Enrollment
{      
    // PRIMARY KEY
    [Key]
    public Guid Guid { get; set; } = Guid.NewGuid();

    [Required]
    public Guid UserGuid { get; set; }

    // Relationship to Parent (User)
    [JsonIgnore]
    [ForeignKey("UserGuid")]
    public User ? User { get; set; }

    // TELLER DATA
    [Required]
    public string EnrollmentId { get; set; } = string.Empty;

    //Set on successful enrollment
    [Required]
    public string UserId { get; set; } = string.Empty;
    // Teller webhook for status
    [Required]
    public string Status { get; set; } = string.Empty;
    // TODO: Encrypt this field
    [Required]
    public string AccessToken { get; set; } = string.Empty;
    [Required]
    public string InstitutionName { get; set; } = string.Empty;

    public string ? InstitutionId { get; set; } = string.Empty;

    public DateTime LastSyncedAt { get; set; } = DateTime.UtcNow;
    
    // One "Enrollment" can have many "BankAccounts"
    public ICollection<BankAccount> Accounts { get; set; } = new List<BankAccount>();

}