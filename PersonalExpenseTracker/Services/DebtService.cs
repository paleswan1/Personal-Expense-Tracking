using PersonalExpenseTracker.DTOs.Debts;
using PersonalExpenseTracker.Models;
using PersonalExpenseTracker.Repositories;
using PersonalExpenseTracker.Services.Interfaces;

namespace PersonalExpenseTracker.Services;

public class DebtService(IGenericRepository genericRepository, IUserService userService) : IDebtService
{
    private static string appDataDirectoryPath = ExtensionMethods.GetAppDirectoryPath();
    private static string appDebtsFilePath = ExtensionMethods.GetAppDebtsFilePath();

    public GetDebtDto GetDebtById(Guid Id)
    {
        var debts = genericRepository.GetAll<Debt>(appDebtsFilePath);

        var debt = debts.FirstOrDefault(x => x.Id == Id);

        if (debt == null)
        {
            throw new Exception("A debt with the following identifier couldn't be found.");
        }

        return new GetDebtDto
        {
            Id = debt.Id,
            Title = debt.Title,
            Source = debt.Source,
            Amount = debt.Amount,
            DueDate = debt.DueDate,
            Status = debt.Status,
        };
    }

    public List<GetDebtDto> GetTransactions()
    {
        var debts = genericRepository.GetAll<Debt>(appDebtsFilePath);
        var userDetails = userService.GetUserDetails();

        if (userDetails == null)
        {
            throw new Exception("You are not logged in.");
        }

        debts = debts.Where(x => x.CreatedBy == userDetails.Id).ToList();

        return debts.Select(debt => new GetDebtDto
        {
            Id = debt.Id,
            Title = debt.Title,
            Source = debt.Source,
            Amount = debt.Amount,
            DueDate = debt.DueDate,
            Status = debt.Status,
        }).ToList();
    }

    public void InsertDebt(InsertDebtDto debt)
    {
        var userDetails = userService.GetUserDetails();

        if (userDetails == null)
        {
            throw new Exception("You are not logged in.");
        }

        var debtModel = new Debt
        {
            Id = Guid.NewGuid(),
            Title = debt.Title,
            Source = debt.Source,
            Amount = debt.Amount,
            DueDate = debt.DueDate,
            Status = false,
            CreatedBy = userDetails.Id,
            CreatedAt = DateTime.Now,
        };

        var debts = genericRepository.GetAll<Debt>(appDebtsFilePath);

        debts.Add(debtModel);

        genericRepository.SaveAll(debts, appDataDirectoryPath, appDebtsFilePath);
    }

    public void UpdateDebt(UpdateDebtDto debt)
    {
        var userDetails = userService.GetUserDetails();

        if (userDetails == null)
        {
            throw new Exception("You are not logged in.");
        }

        var debts = genericRepository.GetAll<Debt>(appDebtsFilePath);

        var debtModel = debts.FirstOrDefault(x => x.Id == debt.Id);

        if (debtModel == null)
        {
            throw new Exception("A debt with the following identifier couldn't be found.");
        }

        debtModel.Title = debt.Title;
        debtModel.Source = debt.Source;
        debtModel.Amount = debt.Amount;
        debtModel.DueDate = debt.DueDate;
        debtModel.Status = debt.Status;
        debtModel.LastModifiedBy = userDetails.Id;
        debtModel.LastModifiedAt = DateTime.Now;

        genericRepository.SaveAll(debts, appDataDirectoryPath, appDebtsFilePath);
    }

    public void MarkDebtAsPaid(Guid debtId)
    {
        var userDetails = userService.GetUserDetails();

        if (userDetails == null)
        {
            throw new Exception("You are not logged in.");
        }

        var debts = genericRepository.GetAll<Debt>(appDebtsFilePath);

        var debtModel = debts.FirstOrDefault(x => x.Id == debtId);

        if (debtModel == null)
        {
            throw new Exception("A debt with the following identifier couldn't be found.");
        }

        debtModel.Status = true;
        debtModel.LastModifiedBy = userDetails.Id;
        debtModel.LastModifiedAt = DateTime.Now;

        genericRepository.SaveAll(debts, appDataDirectoryPath, appDebtsFilePath);
    }

    public void ActivateDeactivateDebt(ActivateDeactivateDebtDto debt)
    {
        var debts = genericRepository.GetAll<Debt>(appDebtsFilePath);

        var debtModel = debts.FirstOrDefault(x => x.Id == debt.Id);

        if (debtModel == null)
        {
            throw new Exception("A debt with the following identifier couldn't be found.");
        }

        debts.Remove(debtModel);

        genericRepository.SaveAll(debts, appDataDirectoryPath, appDebtsFilePath);
    }

}
