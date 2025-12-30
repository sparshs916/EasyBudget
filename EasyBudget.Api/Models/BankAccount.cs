using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EasyBudget.Api.Models;

public class BankAccount
{    
    // Primary Key
    [Key]
    public Guid Guid { get; set; } = Guid.NewGuid();

    // Foreign Key to Parent (Enrollment) + Navigation Property
    [Required]
    public Guid EnrollmentGuid { get; set; }
    [JsonIgnore] 
    [ForeignKey("EnrollmentGuid")]
    public Enrollment? Enrollment { get; set; }
    
    // Teller Data
    [Required]
    public string EnrollmentId { get; set; } = string.Empty;

    [Required]
    public string AccountId { get; set; } = string.Empty; 

    [Required]
    public string InstitutionId { get; set; } = string.Empty;

    [Required]
    public string InstitutionName { get; set; } = string.Empty;

    [Required]
    public string AccountName { get; set; } = string.Empty;

    [Required]
    public string Type { get; set; } = string.Empty;

    [Required]
    public string Subtype { get; set; } = string.Empty;

    [Required]
    public string Currency { get; set; } = string.Empty;

    [Required]
    public string LastFour { get; set; } = string.Empty;

    [Required]
    public string Status { get; set; } = string.Empty;
    
    public DateTime LastSyncedAt { get; set; } = DateTime.UtcNow;

    // One "BankAccount" has many "Transactions", "Balances", etc.
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public ICollection<Balance> Balances { get; set; } = new List<Balance>();

}