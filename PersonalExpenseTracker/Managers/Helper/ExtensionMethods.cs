using System.Security.Cryptography;

namespace PersonalExpenseTracker.Managers.Helper;

public static class ExtensionMethods
{
    private const char SegmentDelimiter = ':';

    public static string HashSecret(this string input)
    {
        const int keySize = 32;
        const int saltSize = 16;
        const int iterations = 100_000;
        
        var algorithm = HashAlgorithmName.SHA256;
        var salt = RandomNumberGenerator.GetBytes(saltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(input, salt, iterations, algorithm, keySize);

        var result = string.Join( 
            SegmentDelimiter,
            Convert.ToHexString(hash),
            Convert.ToHexString(salt),
            iterations,
            algorithm
        );

        return result;
    }

    public static bool VerifyHash(this string input, string hashString)
    {
        var segments = hashString.Split(SegmentDelimiter);
        var hash = Convert.FromHexString(segments[0]);
        var salt = Convert.FromHexString(segments[1]);
        var iterations = int.Parse(segments[2]);
        var algorithm = new HashAlgorithmName(segments[3]);
        var inputHash = Rfc2898DeriveBytes.Pbkdf2(
            input,
            salt,
            iterations,
            algorithm,
            hash.Length
        );

        return CryptographicOperations.FixedTimeEquals(inputHash, hash);
    }

    /// <summary>
    /// Initializing a method to retrieve the directory path to store all the records and logs
    /// </summary>
    /// <returns>Path of the directory that holds all the application data</returns>
    public static string GetAppDirectoryPath()
    {
        return @"D:\Client Work\22068140 Paleswan Shrestha\Personal-Expense-Tracking\PersonalExpenseTracker\wwwroot\data";
    }

    public static string GetAppUsersFilePath()
    {
        return Path.Combine(GetAppDirectoryPath(), "users.json");
    }

    public static string GetAppTransactionsFilePath()
    {
        return Path.Combine(GetAppDirectoryPath(), "transactions.json");
    }

    public static string GetAppDebtsFilePath()
    {
        return Path.Combine(GetAppDirectoryPath(), "debts.json");
    }

    public static string GetAppTagsFilePath()
    {
        return Path.Combine(GetAppDirectoryPath(), "tags.json");
    }

    public static string GetAppTransactionTagsFilePath()
    {
        return Path.Combine(GetAppDirectoryPath(), "transaction-tags.json");
    }
}
