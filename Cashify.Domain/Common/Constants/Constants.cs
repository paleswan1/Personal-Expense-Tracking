namespace Cashify.Domain.Common.Constants;

public abstract class Constants
{
    public abstract class RootDirectory
    {
        public static string Path => @"D:\Client Work\22068140 Paleswan Shrestha\Application Development\Personal-Expense-Tracking\Cashify\wwwroot\data";
    }
    
    public abstract class ModelPath
    {
        public static string Debts => Path.Combine(RootDirectory.Path, "debts.json");
        public static string DebtSources => Path.Combine(RootDirectory.Path, "debt-sources.json");
        public static string Tags => Path.Combine(RootDirectory.Path, "tags.json");
        public static string Transactions => Path.Combine(RootDirectory.Path, "transactions.json");
        public static string TransactionTags => Path.Combine(RootDirectory.Path, "transaction-tags.json");
        public static string Users => Path.Combine(RootDirectory.Path, "users.json");
        public static string TransactionDetails => Path.Combine(RootDirectory.Path, "transactions.csv");
    }
    
    public abstract class Authentication
    {
        public static string Token => "token";
    }
}