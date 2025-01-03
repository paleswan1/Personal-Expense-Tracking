using PersonalExpenseTracker.Models;
using PersonalExpenseTracker.DTOs.Debts;
using PersonalExpenseTracker.Repositories;
using PersonalExpenseTracker.Models.Constant;
using PersonalExpenseTracker.Services.Interfaces;

namespace PersonalExpenseTracker.Services;

public class DebtService(IGenericRepository genericRepository, IUserService userService) : IDebtService
{
    public GetDebtDto GetDebtById(Guid id)
    {
        var debts = genericRepository.GetAll<Debt>(Constants.FilePath.AppDebtsDirectoryPath);

        var debt = debts.FirstOrDefault(x => x.Id == id);

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

    public List<GetDebtDto> GetDebts()
    {
        var debts = genericRepository.GetAll<Debt>(Constants.FilePath.AppDebtsDirectoryPath);
        
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
            Status = DebtStatus.Pending,
            CreatedBy = userDetails.Id,
            CreatedAt = DateTime.Now,
        };

        var debts = genericRepository.GetAll<Debt>(Constants.FilePath.AppDebtsDirectoryPath);

        debts.Add(debtModel);

        genericRepository.SaveAll(debts, Constants.FilePath.AppDataDirectoryPath, Constants.FilePath.AppDebtsDirectoryPath);
    }

    public void UpdateDebt(UpdateDebtDto debt)
    {
        var userDetails = userService.GetUserDetails();

        if (userDetails == null)
        {
            throw new Exception("You are not logged in.");
        }

        var debts = genericRepository.GetAll<Debt>(Constants.FilePath.AppDebtsDirectoryPath);

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

        genericRepository.SaveAll(debts, Constants.FilePath.AppDataDirectoryPath, Constants.FilePath.AppDebtsDirectoryPath);
    }

    public void ClearDebt(Guid debtId)
    {
        var userDetails = userService.GetUserDetails();

        if (userDetails == null)
        {
            throw new Exception("You are not logged in.");
        }

        var debts = genericRepository.GetAll<Debt>(Constants.FilePath.AppDebtsDirectoryPath);

        var debtModel = debts.FirstOrDefault(x => x.Id == debtId);

        if (debtModel == null)
        {
            throw new Exception("A debt with the following identifier couldn't be found.");
        }

        if (debtModel.Status == DebtStatus.Cleared)
        {
            throw new Exception("You can not clear an already cleared debt.");
        }
        
        debtModel.Status = DebtStatus.Cleared;
        debtModel.LastModifiedBy = userDetails.Id;
        debtModel.LastModifiedAt = DateTime.Now;

        genericRepository.SaveAll(debts, Constants.FilePath.AppDataDirectoryPath, Constants.FilePath.AppDebtsDirectoryPath);
    }

    public void ActivateDeactivateDebt(ActivateDeactivateDebtDto debt)
    {
        var debts = genericRepository.GetAll<Debt>(Constants.FilePath.AppDebtsDirectoryPath);

        var debtModel = debts.FirstOrDefault(x => x.Id == debt.Id);

        if (debtModel == null)
        {
            throw new Exception("A debt with the following identifier couldn't be found.");
        }

        debts.Remove(debtModel);

        genericRepository.SaveAll(debts, Constants.FilePath.AppDataDirectoryPath, Constants.FilePath.AppDebtsDirectoryPath);
    }
}
