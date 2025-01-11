using Cashify.Domain.Models;
using Cashify.Domain.Common.Constants;

namespace Cashify.Application.Utility;

public static class EntityHandler
{
    public static string ToFilePath<TEntity>(this TEntity entity) where TEntity : class?
    {
        return entity switch
        {
            Debt => Constants.ModelPath.Debts,
            DebtSource => Constants.ModelPath.DebtSources,
            Tag => Constants.ModelPath.Tags,
            Transaction => Constants.ModelPath.Transactions,
            TransactionTags => Constants.ModelPath.TransactionTags,
            User => Constants.ModelPath.Users,
            _ => throw new Exception("The following model could not be casted to its respective entity object.")
        };
    }
}