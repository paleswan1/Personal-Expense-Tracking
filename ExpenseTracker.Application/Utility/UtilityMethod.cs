using System.Text;
using System.Security.Cryptography;
using ExpenseTracker.Domain.Common.Constants;
using MudBlazor.Utilities;

namespace ExpenseTracker.Application.Utility;

public static class UtilityMethod
{
    /// <summary>
    /// Hashes a respective string in SHA256 Format by computing its byte array values.
    /// </summary>
    /// <param name="input">The input string (or a key-phrase) to be hashed or encrypted.</param>
    /// <returns>A hashed (or an encrypted) string value.</returns>
    public static string Hash(this string input)
    {
        var bytes = Encoding.UTF8.GetBytes(input);
        var hash = SHA256.HashData(bytes);
        return Convert.ToBase64String(hash);
    }

    /// <summary>
    /// Examines the input password and the hashed password to determine if they are the equal or not.
    /// </summary>
    /// <param name="plainInput">The source input value to be examined with a respective encrypted string.</param>
    /// <param name="hashedInput">The destination value to be examined against a source decrypted string.</param>
    /// <returns>The status for the hashing algorithm, whether it matches or not.</returns>
    public static bool Verify(this string plainInput, string hashedInput)
    {
        return Hash(plainInput) == hashedInput;
    }
    
    /// <summary>
    /// Examines the property of a MudColor object and casts it to the respective hex code.
    /// </summary>
    /// <param name="color">Primary MudColor object to handle and manipulate to a hex code.</param>
    /// <returns>String value of a hex code value in 6-letter characters starting with a "#".</returns>
    public static string ToHexCode(this MudColor color)
    {
        return color.ToString(MudColorOutputFormats.Hex);
    }

    public static void InitializeDataDirectory()
    {
        var rootDirectory = Constants.RootDirectory.Path;

        if (!Directory.Exists(rootDirectory))
        {
            Directory.CreateDirectory(rootDirectory);
        }
    }
}