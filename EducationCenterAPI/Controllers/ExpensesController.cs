using EducationCenterAPI.Dtos;
using EducationCenterAPI.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EducationCenterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly IExpenseService _expenseService;
        public ExpensesController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<PagedList<OtherExpenseDto>> GetAllOtherExpenses(int page, int pageSize, string? sortBy, string? sortOrder, string? fromDate, string? toDate)
        {
            return await _expenseService.GetOtherExpensesAsync(page, pageSize, sortBy, sortOrder, fromDate, toDate);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PayOtherExpense(PayOtherExpenseDto payOtherExpenseDto)
        {

            await _expenseService.PayOtherExpenseAsync(payOtherExpenseDto);
            return Ok();
        }
        [HttpPut("{expenseId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateOtherExpense(int expenseId, UpdateOtherExpenseDto updateOtherExpenseDto)
        {
            updateOtherExpenseDto.Id = expenseId;
            await _expenseService.UpdateOtherExpenseAsync(updateOtherExpenseDto);
            return Ok();
        }
    }
}
