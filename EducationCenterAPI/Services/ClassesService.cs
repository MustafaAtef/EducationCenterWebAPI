using System;
using EducationCenterAPI.Database;
using EducationCenterAPI.Database.Entities;
using EducationCenterAPI.Dtos;
using EducationCenterAPI.Exceptions;
using EducationCenterAPI.ServiceContracts;
using Microsoft.EntityFrameworkCore;

namespace EducationCenterAPI.Services;

public class ClassesService : IClassesService
{
    private readonly AppDbContext _dbContext;
    public ClassesService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateClassAsync(CreateClassDto createClassDto)
    {
        // check if subjectteacherid not exists
        var subjectTeacher = await _dbContext.SubjectsTeachers.SingleOrDefaultAsync(st => st.Id == createClassDto.SubjectTeacherId);
        if (subjectTeacher is null)
        {
            throw new BadRequestException("Subject teacher not found");
        }
        // make an array of all dates that this class will be added to 
        var weekDates = _currentWeekDates();
        var allDaysDates = new List<DateOnly>();
        for (int i = 0; i < (int)createClassDto.Repeats; i++)
        {
            foreach (var day in createClassDto.Days)
            {
                var date = weekDates[(day - 1) % 7].AddDays(i * 7);
                allDaysDates.Add(date);
            }
        }
        // check every date in the array if the date with subjectteacherid is exists
        var classesAlreadyExisted = await _dbContext.Classes
            .Where(c => c.SubjectTeacherId == createClassDto.SubjectTeacherId && allDaysDates.Contains(c.Date))
            .CountAsync();
        if (classesAlreadyExisted > 0)
        {
            throw new UniqueException("Class already exists for this subject teacher on the selected dates");
        }
        // add the classes to the database
        _dbContext.Classes.AddRange(allDaysDates.Select(date => new Class
        {
            SubjectTeacherId = createClassDto.SubjectTeacherId.Value,
            Date = date,
            FromTime = createClassDto.FromTime,
            Totime = createClassDto.ToTime,
            AttendanceRegistered = AttendanceRegistered.NotRegistered
        }));
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<ClassDto>> GetAllClassesAsync(int weekOffset, int? gradeId)
    {
        var weekDates = _currentWeekDates(weekOffset);
        var classesQuery = _dbContext.Classes.AsQueryable();
        if (gradeId is not null) classesQuery = classesQuery.Where(c => c.SubjectTeacher.Subject.GradeId == gradeId);

        return await classesQuery.Where(c => c.Date >= weekDates[0] && c.Date <= weekDates[6])
            .Select(c => new ClassDto
            {
                Id = c.Id,
                GradeId = c.SubjectTeacher.Subject.GradeId,
                Grade = c.SubjectTeacher.Subject.Grade.Name,
                SubjectTeacherId = c.SubjectTeacherId,
                Subject = c.SubjectTeacher.Subject.Name,
                Teacher = c.SubjectTeacher.Teacher.Name,
                FromTime = c.FromTime,
                ToTime = c.Totime,
                Date = c.Date,
                AttendanceRegistered = (int)c.AttendanceRegistered
            })
            .OrderBy(c => c.Date).ThenBy(c => c.FromTime)
            .ToListAsync();
    }

    public async Task<ClassAttendanceStatisticsDto> GetClassAttendanceStatisticsAsync(int classId)
    {
        var existedClass = await _dbContext.Classes
            .Include(c => c.SubjectTeacher)
            .ThenInclude(st => st.Subject)
            .ThenInclude(s => s.Grade)
            .Include(c => c.SubjectTeacher)
            .ThenInclude(st => st.Teacher)
            .SingleOrDefaultAsync(c => c.Id == classId);
        if (existedClass is null)
        {
            throw new BadRequestException("Class not found");
        }

        AttendanceStatisticsDto attStats = (await _dbContext.AttendanceStatisticsDto
            .FromSql($"EXEC GetClassAttendanceStats {classId}")
            .ToListAsync())[0];
        if (attStats is null)
            throw new BadRequestException("Class attendance statistics not found");
        return new ClassAttendanceStatisticsDto
        {
            Id = existedClass.Id,
            GradeId = existedClass.SubjectTeacher.Subject.GradeId,
            Grade = existedClass.SubjectTeacher.Subject.Grade.Name,
            SubjectTeacherId = existedClass.SubjectTeacherId,
            Subject = existedClass.SubjectTeacher.Subject.Name,
            Teacher = existedClass.SubjectTeacher.Teacher.Name,
            Date = existedClass.Date,
            FromTime = existedClass.FromTime,
            ToTime = existedClass.Totime,
            AttendanceRegistered = (int)existedClass.AttendanceRegistered,
            TotalStudents = attStats.TotalStudents,
            Presents = attStats.Presents,
            Absents = attStats.Absents
        };
    }

    public async Task<IEnumerable<ClassDto>> GetTodayClassesAsync()
    {
        return await _dbContext.Classes.Where(c => c.Date == DateOnly.FromDateTime(DateTime.Today))
            .Select(c => new ClassDto
            {
                Id = c.Id,
                GradeId = c.SubjectTeacher.Subject.GradeId,
                Grade = c.SubjectTeacher.Subject.Grade.Name,
                SubjectTeacherId = c.SubjectTeacherId,
                Subject = c.SubjectTeacher.Subject.Name,
                Teacher = c.SubjectTeacher.Teacher.Name,
                FromTime = c.FromTime,
                ToTime = c.Totime,
                Date = c.Date,
                AttendanceRegistered = (int)c.AttendanceRegistered
            })
            .OrderBy(c => c.Date).ThenBy(c => c.FromTime)
            .ToListAsync();
    }

    public async Task UpdateClassAsync(UpdateClassDto updateClassDto)
    {
        // check if the class exists
        var classToUpdate = await _dbContext.Classes.SingleOrDefaultAsync(c => c.Id == updateClassDto.Id);
        if (classToUpdate is null)
        {
            throw new BadRequestException("Class not found");
        }
        // check if the subjectteacherid exists
        var subjectTeacher = await _dbContext.SubjectsTeachers.SingleOrDefaultAsync(st => st.Id == updateClassDto.SubjectTeacherId);
        if (subjectTeacher is null)
        {
            throw new BadRequestException("Provided subjectTeacherId not found");
        }
        // check if there is a class int e the same date with the same subjectteacherid
        if (classToUpdate.SubjectTeacherId != updateClassDto.SubjectTeacherId)
        {
            var classAlreadyExisted = await _dbContext.Classes
                .Where(c => c.SubjectTeacherId == updateClassDto.SubjectTeacherId && c.Date == classToUpdate.Date)
                .CountAsync();
            if (classAlreadyExisted > 0)
            {
                throw new UniqueException("Class already exists for this subject and teacher on this date");
            }
        }
        // update the class
        classToUpdate.SubjectTeacherId = updateClassDto.SubjectTeacherId.Value;
        classToUpdate.FromTime = updateClassDto.FromTime;
        classToUpdate.Totime = updateClassDto.ToTime;
        await _dbContext.SaveChangesAsync();
    }

    private List<DateOnly> _currentWeekDates(int weekOffset = 0)
    {
        var today = DateOnly.FromDateTime(DateTime.Today.AddDays(weekOffset * 7));
        var dayOfWeek = (int)today.DayOfWeek;
        var daysToSaturday = (dayOfWeek + 1) % 7; // Calculate days to the previous Saturday
        var saturday = today.AddDays(-daysToSaturday);
        var weekDates = new List<DateOnly>();
        for (int i = 0; i < 7; i++)
        {
            weekDates.Add(saturday.AddDays(i));
        }

        return weekDates;
    }
}
