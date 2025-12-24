using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Collections.Generic;


namespace EasyBudget.Api.Models;

public class Transaction
{   
    // Primary Key
    [Key]
    public Guid Guid { get; set; } = Guid.NewGuid();

    // Foreign Key to Parent (BankAccount)
    [Required]
    public Guid AccountGuid { get; set; }
    // Relationship to Parent (BankAccount)
    [JsonIgnore]
    [ForeignKey("AccountGuid")]
    public BankAccount? BankAccount { get; set; }

    // Teller Data
    [Required]
    public string AccountId { get; set; } = string.Empty;

    // Unique Teller Transaction ID
    [Required]
    public string TransactionId { get; set; } = string.Empty; 

    [Required]
    public decimal Amount { get; set; } = 0.0m; 

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    public DateOnly Date { get; set; }
    
    public string? Category { get; set; } = string.Empty;

    public string ? ProcessingStatus { get; set; } = string.Empty;

    public string ? CounterpartyName { get; set; } = string.Empty;
    
    public string ? CounterpartyType { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty; // posted or pending

    public decimal ? RunningBalance { get; set; } // Financial accuracy

    public string Type { get; set; } = string.Empty; // e.g., "debit" or "credit"
}