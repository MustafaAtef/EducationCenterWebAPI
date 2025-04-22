using System;
using EducationCenterAPI.Database;
using EducationCenterAPI.Database.Entities;
using EducationCenterAPI.Dtos;
using EducationCenterAPI.Exceptions;
using EducationCenterAPI.RepositoryContracts;
using EducationCenterAPI.ServiceContracts;
using Microsoft.EntityFrameworkCore;

namespace EducationCenterAPI.Services;

public class AttendanceService : IAttendanceService
{
    private readonly IUnitOfWork _unitOfWork;
    public AttendanceService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<IEnumerable<ClassStudentDto>> GetAttendanceAsync(int classId)
    {
        //check if classId is valid
        var existingClass = await _unitOfWork.Classes.FindAsync(c => c.Id == classId);
        if (existingClass == null)
        {
            throw new BadRequestException("Class not found");
        }
        //get all students in the class
        return await _unitOfWork.Classes.GetClassStudentsWithAttendanceStatusAsync(classId);
    }

    public async Task<IEnumerable<ClassStudentDto>> GetTodayAttendanceAsync()
    {
        return await _unitOfWork.Classes.GetTodayClassesStudentsWithAttendanceStatusAsync();
    }

    public async Task RegisterStudentAttendanceAsync(RegisterStudentAttendanceAsyncDto takeStudentAttendanceDto)
    {
        // check if the class exists
        var existingClass = await _unitOfWork.Classes.FindAsync(c => c.Id == takeStudentAttendanceDto.ClassId);
        if (existingClass == null)
        {
            throw new BadRequestException("Class not found");
        }
        // check if teh student exists
        var existingStudent = await _unitOfWork.Students.FindAsync(s => s.Id == takeStudentAttendanceDto.StudentId, new string[] { "StudentSubjectsTeachers" });
        if (existingStudent == null)
        {
            throw new BadRequestException("Student not found");
        }
        // check if the student in the subject teached by the teacher of the choosen class (subjectteacher of the class)
        if (!existingStudent.StudentSubjectsTeachers.Any(sst => sst.SubjectTeacherId == existingClass.SubjectTeacherId))
        {
            throw new BadRequestException("This student is not in the choosen class");
        }
        var existedAttendance = await _unitOfWork.Attendances.FindAsync(a => a.ClassId == existingClass.Id && a.StudentId == existingStudent.Id);
        if (existedAttendance != null)
            existedAttendance.IsPresent = takeStudentAttendanceDto.IsPresent.Value == 1;
        else
            _unitOfWork.Attendances.Add(new()
            {
                Class = existingClass,
                Student = existingStudent,
                IsPresent = takeStudentAttendanceDto.IsPresent.Value == 1
            });
        existingClass.AttendanceRegistered = AttendanceRegistered.Registered;
        await _unitOfWork.SaveChangesAsync();
    }
}
