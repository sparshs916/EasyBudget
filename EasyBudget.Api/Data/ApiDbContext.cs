using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using EasyBudget.Api.Models;
using EasyBudget.Api.Services.Interfaces;

namespace EasyBudget.Api.Data;

public class ApiDbContext : DbContext
{
    private readonly IEncryptionService? _encryptionService;

    public ApiDbContext(DbContextOptions<ApiDbContext> options,
        IEncryptionService? encryptionService = null)
        : base(options)
    {
        _encryptionService = encryptionService;
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public DbSet<BankAccount> BankAccounts => Set<BankAccount>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Balance> Balances => Set<Balance>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Skip encryption converters at design time (when _encryptionService is null)
        if (_encryptionService is null)
            return;

        var encryptionConverter = new ValueConverter<string, string>(
            v => _encryptionService.Encrypt(v),
            v => _encryptionService.Decrypt(v)
        );

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(u => u.Email).HasConversion(encryptionConverter);
            entity.Property(u => u.Username).HasConversion(encryptionConverter);
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.Property(e => e.AccessToken).HasConversion(encryptionConverter);
            entity.Property(e => e.InstitutionName).HasConversion(encryptionConverter);
        });

        modelBuilder.Entity<BankAccount>(entity =>
        {
            entity.Property(b => b.AccountId).HasConversion(encryptionConverter);
            entity.Property(b => b.AccountName).HasConversion(encryptionConverter);
            entity.Property(b => b.LastFour).HasConversion(encryptionConverter);
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.Property(t => t.Description).HasConversion(encryptionConverter);
            entity.Property(t => t.CounterpartyName).HasConversion(encryptionConverter);
            entity.Property(t => t.AccountId).HasConversion(encryptionConverter);
        });
    }
}