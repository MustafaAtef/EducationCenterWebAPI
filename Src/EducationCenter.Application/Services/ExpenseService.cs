using System.Linq.Expressions;
using EducationCenter.Core;
using EducationCenter.Core.Entities;
using EducationCenter.Application.Dtos;
using EducationCenter.Application.ServiceContracts;
using EducationCenter.Core.RepositoryContracts;

namespace EducationCenter.Application.Services;

public class ExpenseService : IExpenseService
{
    private readonly IUnitOfWork _unitOfWork;
    public ExpenseService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<PagedList<OtherExpenseDto>> GetOtherExpensesAsync(int page, int pageSize, string? sortBy, string? sortOrder, string? fromDate, string? toDate)
    {
        Expression<Func<OtherExpense, bool>>? predicate = null;
        if (!string.IsNullOrEmpty(fromDate) && DateTime.TryParse(fromDate, out var fromDateParsed) && !string.IsNullOrEmpty(toDate) && DateTime.TryParse(toDate, out var toDateParsed))
        {
            predicate = e => e.PaidAt >= fromDateParsed && e.PaidAt <= toDateParsed;
        }
        Expression<Func<OtherExpense, object>> keySelector;
        switch (sortBy?.ToLower())
        {
            case "amount":
                keySelector = e => e.Amount;
                break;
            case "createdat":
                keySelector = e => e.PaidAt;
                break;
            default:
                keySelector = e => e.PaidAt;
                break;
        }
        var otherExpenses = await _unitOfWork.OtherExpenses.FindAllAsync(page, pageSize, predicate, keySelector, sortOrder);
        var totalOtherExpenses = await _unitOfWork.OtherExpenses.CountAsync(predicate);

        return new PagedList<OtherExpenseDto>(totalOtherExpenses, pageSize, page, otherExpenses.Select(e => new OtherExpenseDto
        {
            Id = e.Id,
            Amount = e.Amount,
            Notes = e.Notes,
            PaidAt = e.PaidAt,
        }));
    }

    public async Task PayOtherExpenseAsync(PayOtherExpenseDto payOtherExpenseDto)
    {
        var expense = new Expense
        {
            ExpenseTypeId = (int)ExpenseTypeEnum.OhterExpense,
            Paid = payOtherExpenseDto.Amount
        };
        var otherExpense = new OtherExpense
        {
            Amount = payOtherExpenseDto.Amount,
            Notes = payOtherExpenseDto.Notes,
            Expense = expense,
        };
        _unitOfWork.Expenses.Add(expense);
        _unitOfWork.OtherExpenses.Add(otherExpense);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateOtherExpenseAsync(UpdateOtherExpenseDto updateOtherExpenseDto)
    {
        var expense = await _unitOfWork.OtherExpenses.FindAsync(oe => oe.ExpenseId == updateOtherExpenseDto.Id);
        if (expense == null)
        {
            throw new Exception("Expense not found");
        }
        expense.Amount = updateOtherExpenseDto.Amount;
        expense.Notes = updateOtherExpenseDto.Notes;
        _unitOfWork.OtherExpenses.Update(expense);
        await _unitOfWork.SaveChangesAsync();
    }
}
