using System;
using EducationCenter.Application.Dtos;

namespace EducationCenter.Application.ServiceContracts;

public interface IExpenseService
{
    Task PayOtherExpenseAsync(PayOtherExpenseDto payOtherExpenseDto);
    Task UpdateOtherExpenseAsync(UpdateOtherExpenseDto updateOtherExpenseDto);
    Task<PagedList<OtherExpenseDto>> GetOtherExpensesAsync(int page, int pageSize, string? sortBy, string? sortOrder, string? fromDate, string? toDate);

}
