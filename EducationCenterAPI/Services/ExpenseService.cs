using System;
using System.Linq.Expressions;
using EducationCenterAPI.Database;
using EducationCenterAPI.Database.Entities;
using EducationCenterAPI.Dtos;
using EducationCenterAPI.ServiceContracts;
using Microsoft.EntityFrameworkCore;

namespace EducationCenterAPI.Services;

public class ExpenseService : IExpenseService
{
    private readonly AppDbContext _appDbContext;
    public ExpenseService(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    public async Task<PagedList<OtherExpenseDto>> GetOtherExpensesAsync(int page, int pageSize, string? sortBy, string? sortOrder, string? fromDate, string? toDate)
    {
        var query = _appDbContext.OtherExpenses.AsQueryable();
        if (!string.IsNullOrEmpty(fromDate) && DateTime.TryParse(fromDate, out var fromDateParsed) && !string.IsNullOrEmpty(toDate) && DateTime.TryParse(toDate, out var toDateParsed))
        {
            query = query.Where(e => e.PaidAt >= fromDateParsed && e.PaidAt <= toDateParsed);
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

        if (sortOrder?.ToLower() == "asc")
        {
            query = query.OrderBy(keySelector);
        }
        else
        {
            query = query.OrderByDescending(keySelector);
        }

        return await PagedList<OtherExpenseDto>.Create(query.Select(e => new OtherExpenseDto
        {
            Id = e.Id,
            Amount = e.Amount,
            Notes = e.Notes,
            PaidAt = e.PaidAt,
        }), page, pageSize);
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
        _appDbContext.Expenses.Add(expense);
        _appDbContext.OtherExpenses.Add(otherExpense);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task UpdateOtherExpenseAsync(UpdateOtherExpenseDto updateOtherExpenseDto)
    {
        var expense = await _appDbContext.OtherExpenses.SingleOrDefaultAsync(oe => oe.ExpenseId == updateOtherExpenseDto.Id);
        if (expense == null)
        {
            throw new Exception("Expense not found");
        }
        expense.Amount = updateOtherExpenseDto.Amount;
        expense.Notes = updateOtherExpenseDto.Notes;
        _appDbContext.OtherExpenses.Update(expense);
        await _appDbContext.SaveChangesAsync();
    }
}
