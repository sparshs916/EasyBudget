using Microsoft.AspNetCore.DataProtection;
using EasyBudget.Api.Services.Interfaces;
using DotNetEnv;
public class EncryptionService(
    IDataProtectionProvider provider
) : IEncryptionService
{
    private readonly IDataProtector _protector = provider.CreateProtector(Env.GetString("PROTECTOR_KEY"));

    public string Encrypt(string plainText) => 
        string.IsNullOrEmpty(plainText) ? plainText : _protector.Protect(plainText);

    public string Decrypt(string cipherText)
    {
        if (string.IsNullOrEmpty(cipherText))
        {
            return cipherText;
        }

        try { 
            return _protector.Unprotect(cipherText); 
            }
        catch { 
            return "[Decryption Failed]"; 
        } 
    }
}