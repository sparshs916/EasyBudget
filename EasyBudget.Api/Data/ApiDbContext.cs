using Microsoft.EntityFrameworkCore;
using EasyBudget.Api.Models; // Add this line

namespace EasyBudget.Api.Data;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }

    // This creates the "Users" table in PostgreSQL
    public DbSet<User> Users => Set<User>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public DbSet<BankAccount> BankAccounts => Set<BankAccount>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Balance> Balances => Set<Balance>();
}