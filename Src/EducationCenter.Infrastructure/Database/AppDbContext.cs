﻿using EducationCenter.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EducationCenter.Infrastructure.Database;

public class AppDbContext : DbContext
{
    public AppDbContext()
    {

    }

    public AppDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Grade>(builder =>
        {
            builder.HasIndex(g => g.Name).IsUnique();
            builder.Property(g => g.Name).HasColumnType("nvarchar(100)");
            builder.Property(g => g.CreatedAt).HasDefaultValueSql("getdate()");
        });
        modelBuilder.Entity<User>(builder =>
        {
            builder.HasIndex(user => user.Email).IsUnique();
            builder.Property(user => user.Name).HasColumnType("nvarchar(100)");
            builder.Property(user => user.Email).HasColumnType("nvarchar(100)");
            builder.Property(user => user.Phone).HasColumnType("nvarchar(20)");
            builder.Property(user => user.RefreshToken).HasColumnType("nvarchar(100)");
            builder.Property(user => user.CreatedAt).HasDefaultValueSql("getdate()");
        });
        modelBuilder.Entity<Student>(builder =>
        {
            builder.HasIndex(std => std.Email).IsUnique();
            builder.Property(std => std.Name).HasColumnType("nvarchar(100)");
            builder.Property(std => std.Email).HasColumnType("nvarchar(100)");
            builder.Property(std => std.Phone).HasColumnType("nvarchar(20)");
            builder.Property(std => std.CreatedAt).HasDefaultValueSql("getdate()");
        });
        modelBuilder.Entity<Teacher>(builder =>
       {
           builder
           .HasMany(t => t.Subjects)
           .WithMany(sub => sub.Teachers)
           .UsingEntity<SubjectTeacher>();
           builder.HasIndex(t => t.Email).IsUnique();
           builder.Property(t => t.Name).HasColumnType("nvarchar(100)");
           builder.Property(t => t.Email).HasColumnType("nvarchar(100)");
           builder.Property(t => t.Phone).HasColumnType("nvarchar(20)");
           builder.Property(t => t.CreatedAt).HasDefaultValueSql("getdate()");
       });
        modelBuilder.Entity<Subject>(builder =>
        {
            builder.HasIndex(sub => new { sub.Name, sub.GradeId }).IsUnique();
            builder.Property(sub => sub.Name).HasColumnType("nvarchar(100)");
            builder.Property(sub => sub.CreatedAt).HasDefaultValueSql("getdate()");
        });
        modelBuilder.Entity<StudentSubjectsTeachers>(builder =>
        {
            builder.HasIndex(sst => new { sst.StudentId, sst.SubjectTeacherId }).IsUnique();
            builder.HasOne(sst => sst.Student)
                .WithMany(std => std.StudentSubjectsTeachers)
                .HasForeignKey(sst => sst.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(sst => sst.SubjectTeacher)
                .WithMany()
                .HasForeignKey(sst => sst.SubjectTeacherId);

        });
        modelBuilder.Entity<Class>(builder =>
        {
            builder.HasIndex(c => new { c.SubjectTeacherId, c.Date }).IsUnique();
            builder.Property(c => c.FromTime).HasColumnType("nvarchar(10)");
            builder.Property(c => c.Totime).HasColumnType("nvarchar(10)");
            builder.HasOne(c => c.SubjectTeacher).WithMany()
                .HasForeignKey(c => c.SubjectTeacherId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        modelBuilder.Entity<Attendance>(builder =>
        {
            builder.HasKey(a => new { a.StudentId, a.ClassId });
            builder.Property(a => a.RegisteredAt).HasDefaultValueSql("getdate()");

        });
        modelBuilder.Entity<ClassStudent>(e => { e.HasNoKey().ToView(null); });
        modelBuilder.Entity<AttendanceStatistics>(e => { e.HasNoKey().ToView(null); });
        modelBuilder.Entity<StudentFee>(builder =>
        {
            builder.HasKey(sf => new { sf.Months, sf.StudentId });
            builder.Property(sf => sf.Notes).HasColumnType("nvarchar(250)");
            builder.Property(sf => sf.PaidAt).HasDefaultValueSql("getdate()");
        });
        modelBuilder.Entity<TeacherSalary>(builder =>
        {
            builder.HasKey(ts => new { ts.Months, ts.TeacherId });
            builder.Property(ts => ts.Notes).HasColumnType("nvarchar(250)");
            builder.Property(ts => ts.PaidAt).HasDefaultValueSql("getdate()");
        });
        modelBuilder.Entity<OtherExpense>(builder =>
        {
            builder.Property(e => e.Notes).HasColumnType("nvarchar(250)");
            builder.Property(e => e.PaidAt).HasDefaultValueSql("getdate()");
        });
    }

    public DbSet<Grade> Grades { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<SubjectTeacher> SubjectsTeachers { get; set; }
    public DbSet<StudentSubjectsTeachers> StudentSubjectsTeachers { get; set; }
    public DbSet<Class> Classes { get; set; }
    public DbSet<Attendance> Attendances { get; set; }
    public DbSet<ClassStudent> classStudentDto { get; set; }
    public DbSet<AttendanceStatistics> AttendanceStatisticsDto { get; set; }
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<StudentFee> StudentFees { get; set; }
    public DbSet<TeacherSalary> TeacherSalaries { get; set; }
    public DbSet<OtherExpense> OtherExpenses { get; set; }
}

public static class AppDbContextExtensions
{

    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddDbContext<AppDbContext>(builder =>
        {
            builder.UseSqlServer(configuration.GetConnectionString("sqlserverConnectionString"));
        });
    }
}
