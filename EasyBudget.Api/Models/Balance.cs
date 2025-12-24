using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EasyBudget.Api.Models;

public class Balance
{
    // Primary Key
    [Key]
    public Guid Guid { get; set; } = Guid.NewGuid();

    [Required]
    public Guid AccountGuid { get; set; }
    // Relationship to Parent (BankAccount)
    [JsonIgnore]
    [ForeignKey("AccountGuid")]
    public BankAccount? BankAccount { get; set; }

    // Teller Data
    [Required]
    public string AccountId { get; set; } = string.Empty; // Foreign Key to BankAccount
    
    public decimal Available { get; set; }  = 0.0m;// Amount accessible now
    public decimal Ledger { get; set; } = 0.0m;   // Total amount in the account

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}