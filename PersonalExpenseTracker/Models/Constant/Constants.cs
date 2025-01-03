using PersonalExpenseTracker.Managers.Helper;

namespace PersonalExpenseTracker.Models.Constant;

public abstract class Constants
{
    public abstract class FilePath
    {
        public static readonly string AppDataDirectoryPath = ExtensionMethods.GetAppDirectoryPath();
        public static readonly string AppUsersDirectoryPath = ExtensionMethods.GetAppUsersFilePath();
        public static readonly string AppTransactionsDirectoryPath = ExtensionMethods.GetAppTransactionsFilePath();
        public static readonly string AppDebtsDirectoryPath = ExtensionMethods.GetAppDebtsFilePath();
        public static readonly string AppTagsDirectoryPath = ExtensionMethods.GetAppTagsFilePath();
        public static readonly string AppTransactionTagsDirectoryPath = ExtensionMethods.GetAppTransactionTagsFilePath();
    }
}