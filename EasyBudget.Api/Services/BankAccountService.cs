namespace EasyBudget.Api.Services;

using EasyBudget.Api.DTO;
using EasyBudget.Api.Services.Interfaces;
using EasyBudget.Api.Models;
using EasyBudget.Api.Data;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

public class BankAccountService(
    ILogger<BankAccountService> logger,
    ApiDbContext context,
    ITellerService tellerService
) : IBankAccountService
{

    public async Task<BankAccountDto[]>
    CreateBankAccountAsync(string accessToken,
    string auth0Id, CancellationToken cancellationToken = default)
    {
        try
        {
            // Get all bank accounts for the enrollment 
            BankAccountDto[]? bankAccounts = await
                tellerService.FetchAllBankAccountsAsync(accessToken, cancellationToken);

            if (bankAccounts is null || bankAccounts.Length == 0)
            {
                logger.LogWarning("No bank accounts found to create.");
                return bankAccounts ?? Array.Empty<BankAccountDto>();
            }

            // Check if bank accounts already exist in the database
            var existingAccounts = await context.BankAccounts
                .Where(b => bankAccounts.Select(ba => ba.EnrollmentId).Contains(b.EnrollmentId))
                .ToListAsync(cancellationToken);

            if (existingAccounts.Count > 0)
            {
                logger.LogInformation("Some bank accounts already exist in the database. Skipping creation for those accounts.");
                var existingAccountDtos = existingAccounts
                    .Select(ba => new BankAccountDto(
                        ba.AccountId,
                        ba.AccountName,
                        ba.Currency,
                        ba.EnrollmentId,
                        new InstitutionDto(ba.InstitutionId, ba.InstitutionName),
                        ba.LastFour,
                        ba.Subtype,
                        ba.Type,
                        ba.Status
                    ))
                    .ToArray();

                return existingAccountDtos;
            }

            foreach (var bankAccountDto in bankAccounts)
            {
                var enrollment = await context.Enrollments
                    .Include(e => e.User)
                    .FirstOrDefaultAsync(e =>
                        e.EnrollmentId == bankAccountDto.EnrollmentId &&
                        e.User != null &&
                        e.User.Auth0Id == auth0Id,
                        cancellationToken);

                if (enrollment is null)
                {
                    logger.LogWarning("No enrollment found for EnrollmentId {EnrollmentId}",
                        bankAccountDto.EnrollmentId);
                    continue;
                }

                BankAccount newBankAccount = new BankAccount
                {
                    AccountId = bankAccountDto.AccountId,
                    AccountName = bankAccountDto.AccountName,
                    Currency = bankAccountDto.Currency,
                    EnrollmentId = bankAccountDto.EnrollmentId,
                    EnrollmentGuid = enrollment.Guid,
                    InstitutionId = bankAccountDto.Institution.Id,
                    InstitutionName = bankAccountDto.Institution.Name,
                    LastFour = bankAccountDto.LastFour,
                    Subtype = bankAccountDto.Subtype,
                    Type = bankAccountDto.Type,
                    Status = bankAccountDto.Status,
                };
                context.BankAccounts.Add(newBankAccount);
            }

            await context.SaveChangesAsync(cancellationToken);
            logger.LogInformation("Created {Count} bank accounts", bankAccounts.Length);
            return bankAccounts;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while creating bank accounts in database.");
            return Array.Empty<BankAccountDto>();
        }
    }

    public async Task<BankAccountDto[]>
    GetBankAccountsbyIdAsync(string EnrollmentId,
        string auth0Id, CancellationToken cancellationToken = default)
    {
        var user = await context.Users
            .Where(u => u.Auth0Id == auth0Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            logger.LogWarning("No user found with Auth0Id {Auth0Id}", auth0Id);
            return Array.Empty<BankAccountDto>();
        }

        var bankAccountsList = await context.BankAccounts
            .Where(b => b.EnrollmentId == EnrollmentId)
            .ToListAsync(cancellationToken);

        var bankAccountDtoList = new List<BankAccountDto>();
        foreach (var bankAccount in bankAccountsList)
        {
            var bankAccountDto = new BankAccountDto(
                bankAccount.AccountId,
                bankAccount.AccountName,
                bankAccount.Currency,
                bankAccount.EnrollmentId,
                new InstitutionDto(bankAccount.InstitutionId, bankAccount.InstitutionName),
                bankAccount.LastFour,
                bankAccount.Subtype,
                bankAccount.Type,
                bankAccount.Status
            );
            bankAccountDtoList.Add(bankAccountDto);
        }

        if (bankAccountDtoList.Count == 0)
        {
            logger.LogWarning("No bank accounts found for enrollment {EnrollmentId}", EnrollmentId);
            return Array.Empty<BankAccountDto>();
        }
        return bankAccountDtoList.ToArray();
    }

    public async Task<BankAccountDto[]>
    GetBankAccountsAsync(string auth0Id, CancellationToken cancellationToken = default)
    {
        var user = await context.Users
            .Where(u => u.Auth0Id == auth0Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            logger.LogWarning("No user found with Auth0Id {Auth0Id}", auth0Id);
            return Array.Empty<BankAccountDto>();
        }

        var bankAccountsList = await context.BankAccounts
            .ToListAsync(cancellationToken);

        if (bankAccountsList.Count == 0)
        {
            logger.LogWarning("No bank accounts found");
            return Array.Empty<BankAccountDto>();
        }

        var bankAccountsDto = new List<BankAccountDto>();
        foreach (var bankAccount in bankAccountsList)
        {
            var bankAccountDto = new BankAccountDto(
                bankAccount.AccountId,
                bankAccount.AccountName,
                bankAccount.Currency,
                bankAccount.EnrollmentId,
                new InstitutionDto(bankAccount.InstitutionId, bankAccount.InstitutionName),
                bankAccount.LastFour,
                bankAccount.Subtype,
                bankAccount.Type,
                bankAccount.Status
            );
            bankAccountsDto.Add(bankAccountDto);
        }
        return bankAccountsDto.ToArray();
    }
}