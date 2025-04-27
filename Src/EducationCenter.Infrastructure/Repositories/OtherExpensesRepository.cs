using EducationCenter.Core.Entities;
using EducationCenter.Core.RepositoryContracts;
using EducationCenter.Infrastructure.Database;

namespace EducationCenterAPI.Repositories;

public class OtherExpensesRepository : Repository<OtherExpense>, IOtherExpensesRepository
{
    public OtherExpensesRepository(AppDbContext appDbContext) : base(appDbContext)
    {
    }

}
