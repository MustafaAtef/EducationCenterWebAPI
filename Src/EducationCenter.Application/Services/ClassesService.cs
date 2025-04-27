using System.Linq.Expressions;
using EducationCenter.Core;
using EducationCenter.Core.Entities;
using EducationCenter.Core.Exceptions;
using EducationCenter.Application.Dtos;
using EducationCenter.Application.ServiceContracts;
using EducationCenter.Core.RepositoryContracts;

namespace EducationCenter.Application.Services;

public class ClassesService : IClassesService
{
    private readonly IUnitOfWork _unitOfWork;
    public ClassesService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task CreateClassAsync(CreateClassDto createClassDto)
    {
        // check if subjectteacherid not exists
        var subjectTeacher = await _unitOfWork.SubjectsTeachers.FindAsync(st => st.Id == createClassDto.SubjectTeacherId);
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
        var classesAlreadyExisted = await _unitOfWork.Classes
            .CountAsync(c => c.SubjectTeacherId == createClassDto.SubjectTeacherId && allDaysDates.Contains(c.Date));
        if (classesAlreadyExisted > 0)
        {
            throw new UniqueException("Class already exists for this subject teacher on the selected dates");
        }
        // add the classes to the database
        _unitOfWork.Classes.AddRange(allDaysDates.Select(date => new Class
        {
            SubjectTeacherId = createClassDto.SubjectTeacherId.Value,
            Date = date,
            FromTime = createClassDto.FromTime,
            Totime = createClassDto.ToTime,
            AttendanceRegistered = AttendanceRegistered.NotRegistered
        }));
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<ClassDto>> GetAllClassesAsync(int weekOffset, int? gradeId)
    {
        var weekDates = _currentWeekDates(weekOffset);
        Expression<Func<Class, bool>>? predicate = null;
        if (gradeId is not null)
        {
            predicate = c => c.SubjectTeacher.Subject.GradeId == gradeId && c.Date >= weekDates[0] && c.Date <= weekDates[6];
        }
        else
        {
            predicate = c => c.Date >= weekDates[0] && c.Date <= weekDates[6];
        }
        var classes = await _unitOfWork.Classes.FindAllAsync(1, 7, predicate, c => c.Date, "asc", new[] { "SubjectTeacher.Subject.Grade", "SubjectTeacher.Teacher" });
        return classes.Select(c => new ClassDto
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
        });
    }

    public async Task<ClassAttendanceStatisticsDto> GetClassAttendanceStatisticsAsync(int classId)
    {
        var existedClass = await _unitOfWork.Classes.FindAsync(c => c.Id == classId, new[] { "SubjectTeacher.Subject.Grade", "SubjectTeacher.Teacher" });
        if (existedClass is null)
        {
            throw new BadRequestException("Class not found");
        }

        AttendanceStatistics attStats = (await _unitOfWork.Classes.GetClassAttendanceStats(classId))[0];
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
        return (await _unitOfWork.Classes.FindAllAsync(1, 10000, c => c.Date == DateOnly.FromDateTime(DateTime.Today), c => c.FromTime, "asc", new[] { "SubjectTeacher.Subject.Grade", "SubjectTeacher.Teacher" }))
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
            });
    }

    public async Task UpdateClassAsync(UpdateClassDto updateClassDto)
    {
        // check if the class exists
        var classToUpdate = await _unitOfWork.Classes.FindAsync(c => c.Id == updateClassDto.Id);
        if (classToUpdate is null)
        {
            throw new BadRequestException("Class not found");
        }
        // check if the subjectteacherid exists
        var subjectTeacher = await _unitOfWork.SubjectsTeachers.FindAsync(st => st.Id == updateClassDto.SubjectTeacherId);
        if (subjectTeacher is null)
        {
            throw new BadRequestException("Provided subjectTeacherId not found");
        }
        // check if there is a class int e the same date with the same subjectteacherid
        if (classToUpdate.SubjectTeacherId != updateClassDto.SubjectTeacherId)
        {
            var classAlreadyExisted = await _unitOfWork.Classes
                .CountAsync(c => c.SubjectTeacherId == updateClassDto.SubjectTeacherId && c.Date == classToUpdate.Date);
            if (classAlreadyExisted > 0)
            {
                throw new UniqueException("Class already exists for this subject and teacher on this date");
            }
        }
        // update the class
        classToUpdate.SubjectTeacherId = updateClassDto.SubjectTeacherId.Value;
        classToUpdate.FromTime = updateClassDto.FromTime;
        classToUpdate.Totime = updateClassDto.ToTime;
        await _unitOfWork.SaveChangesAsync();
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
