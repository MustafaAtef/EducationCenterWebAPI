using System;
using EducationCenterAPI.Database;
using EducationCenterAPI.Database.Entities;
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
    public async Task<IEnumerable<ClassStudentDto>> GetAttendanceAsync(int classId)
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

    public async Task<IEnumerable<ClassStudentDto>> GetTodayAttendanceAsync()
    {
        return await _appDbContext.classStudentDto.FromSql($"EXEC GetTodayClassesStudentsWithAttendanceStatus").ToListAsync();
    }

    public async Task RegisterStudentAttendanceAsync(RegisterStudentAttendanceAsyncDto takeStudentAttendanceDto)
    {
        // check if the class exists
        var existingClass = await _appDbContext.Classes.SingleOrDefaultAsync(c => c.Id == takeStudentAttendanceDto.ClassId);
        if (existingClass == null)
        {
            throw new BadRequestException("Class not found");
        }
        // check if teh student exists
        var existingStudent = await _appDbContext.Students.Include(std => std.StudentSubjectsTeachers).SingleOrDefaultAsync(s => s.Id == takeStudentAttendanceDto.StudentId);
        if (existingStudent == null)
        {
            throw new BadRequestException("Student not found");
        }
        // check if the student in the subject teached by the teacher of the choosen class (subjectteacher of the class)
        if (!existingStudent.StudentSubjectsTeachers.Any(sst => sst.SubjectTeacherId == existingClass.SubjectTeacherId))
        {
            throw new BadRequestException("This student is not in the choosen class");
        }
        var existedAttendance = await _appDbContext.Attendances.SingleOrDefaultAsync(a => a.ClassId == existingClass.Id && a.StudentId == existingStudent.Id);
        if (existedAttendance != null)
            existedAttendance.IsPresent = takeStudentAttendanceDto.IsPresent.Value == 1;
        else
            _appDbContext.Attendances.Add(new()
            {
                Class = existingClass,
                Student = existingStudent,
                IsPresent = takeStudentAttendanceDto.IsPresent.Value == 1
            });
        existingClass.AttendanceRegistered = AttendanceRegistered.Registered;
        await _appDbContext.SaveChangesAsync();
    }
}
