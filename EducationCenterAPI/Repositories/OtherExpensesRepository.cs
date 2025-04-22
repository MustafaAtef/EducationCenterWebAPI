using System;
using EducationCenterAPI.Database;
using EducationCenterAPI.Database.Entities;
using EducationCenterAPI.RepositoryContracts;

namespace EducationCenterAPI.Repositories;

public class OtherExpensesRepository : Repository<OtherExpense>, IOtherExpensesRepository
{
    public OtherExpensesRepository(AppDbContext appDbContext) : base(appDbContext)
    {
    }

}
