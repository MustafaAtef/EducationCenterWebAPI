using System;
using EducationCenterAPI.Dtos;

namespace EducationCenterAPI.ServiceContracts;

public interface IExpenseService
{
    Task PayOtherExpenseAsync(PayOtherExpenseDto payOtherExpenseDto);
    Task UpdateOtherExpenseAsync(UpdateOtherExpenseDto updateOtherExpenseDto);
    Task<PagedList<OtherExpenseDto>> GetOtherExpensesAsync(int page, int pageSize, string? sortBy, string? sortOrder, string? fromDate, string? toDate);

}
