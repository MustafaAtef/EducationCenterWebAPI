using System;
using EducationCenterAPI.Database;
using EducationCenterAPI.Dtos;
using EducationCenterAPI.Exceptions;
using EducationCenterAPI.ServiceContracts;
using Microsoft.EntityFrameworkCore;

namespace EducationCenterAPI.Services;

public class AttendanceService : IAttendanceService
{
    private readonly AppDbContext _appDbContext;
    public AttendanceService(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    public async Task<IEnumerable<ClassStudentDto>> GetAttendance(int classId)
    {
        //check if classId is valid
        var existingClass = await _appDbContext.Classes.SingleOrDefaultAsync(c => c.Id == classId);
        if (existingClass == null)
        {
            throw new BadRequestException("Class not found");
        }
        //get all students in the class
        return await _appDbContext.classStudentDto.FromSql($"EXEC GetClassStudentsWithAttendanceStatus {classId}").ToListAsync();
    }
}
