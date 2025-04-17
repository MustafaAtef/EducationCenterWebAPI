﻿// <auto-generated />
using System;
using EducationCenterAPI.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EducationCenterAPI.Database.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250417195720_changeCreatedAtColumnToPaidAtInExpensesTables")]
    partial class changeCreatedAtColumnToPaidAtInExpensesTables
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EducationCenterAPI.Database.Entities.Attendance", b =>
                {
                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.Property<int>("ClassId")
                        .HasColumnType("int");

                    b.Property<bool>("IsPresent")
                        .HasColumnType("bit");

                    b.Property<DateTime>("RegisteredAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.HasKey("StudentId", "ClassId");

                    b.HasIndex("ClassId");

                    b.ToTable("Attendances");
                });

            modelBuilder.Entity("EducationCenterAPI.Database.Entities.Class", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AttendanceRegistered")
                        .HasColumnType("int");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<string>("FromTime")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)");

                    b.Property<int>("SubjectTeacherId")
                        .HasColumnType("int");

                    b.Property<string>("Totime")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("Id");

                    b.HasIndex("SubjectTeacherId", "Date")
                        .IsUnique();

                    b.ToTable("Classes");
                });

            modelBuilder.Entity("EducationCenterAPI.Database.Entities.Expense", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ExpenseTypeId")
                        .HasColumnType("int");

                    b.Property<decimal>("Paid")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("PaidAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ExpenseTypeId");

                    b.ToTable("Expenses");
                });

            modelBuilder.Entity("EducationCenterAPI.Database.Entities.ExpenseType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ExpenseType");
                });

            modelBuilder.Entity("EducationCenterAPI.Database.Entities.Grade", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Grades");
                });

            modelBuilder.Entity("EducationCenterAPI.Database.Entities.OtherExpense", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ExpenseId")
                        .HasColumnType("int");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)");

                    b.Property<DateTime>("PaidAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.HasKey("Id");

                    b.HasIndex("ExpenseId");

                    b.ToTable("OtherExpenses");
                });

            modelBuilder.Entity("EducationCenterAPI.Database.Entities.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("GradeId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("GradeId");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("EducationCenterAPI.Database.Entities.StudentFee", b =>
                {
                    b.Property<DateOnly>("Months")
                        .HasColumnType("date");

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ExpenseId")
                        .HasColumnType("int");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)");

                    b.Property<decimal>("Paid")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("PaidAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.HasKey("Months", "StudentId");

                    b.HasIndex("ExpenseId");

                    b.HasIndex("StudentId");

                    b.ToTable("StudentFees");
                });

            modelBuilder.Entity("EducationCenterAPI.Database.Entities.StudentSubjectsTeachers", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.Property<int>("SubjectTeacherId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SubjectTeacherId");

                    b.HasIndex("StudentId", "SubjectTeacherId")
                        .IsUnique();

                    b.ToTable("StudentSubjectsTeachers");
                });

            modelBuilder.Entity("EducationCenterAPI.Database.Entities.Subject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<int>("GradeId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("GradeId");

                    b.HasIndex("Name", "GradeId")
                        .IsUnique();

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("EducationCenterAPI.Database.Entities.SubjectTeacher", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("SubjectsId")
                        .HasColumnType("int");

                    b.Property<int>("TeachersId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SubjectsId");

                    b.HasIndex("TeachersId");

                    b.ToTable("SubjectsTeachers");
                });

            modelBuilder.Entity("EducationCenterAPI.Database.Entities.Teacher", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Teachers");
                });

            modelBuilder.Entity("EducationCenterAPI.Database.Entities.TeacherSalary", b =>
                {
                    b.Property<DateOnly>("Months")
                        .HasColumnType("date");

                    b.Property<int>("TeacherId")
                        .HasColumnType("int");

                    b.Property<int>("ExpenseId")
                        .HasColumnType("int");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)");

                    b.Property<decimal>("Paid")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("PaidAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<decimal>("Salary")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Months", "TeacherId");

                    b.HasIndex("ExpenseId");

                    b.HasIndex("TeacherId");

                    b.ToTable("TeacherSalaries");
                });

            modelBuilder.Entity("EducationCenterAPI.Database.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("RefreshTokenExpirationDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("EducationCenterAPI.Dtos.AttendanceStatisticsDto", b =>
                {
                    b.Property<int>("Absents")
                        .HasColumnType("int");

                    b.Property<int>("Presents")
                        .HasColumnType("int");

                    b.Property<int>("TotalStudents")
                        .HasColumnType("int");

                    b.ToTable((string)null);

                    b.ToView(null, (string)null);
                });

            modelBuilder.Entity("EducationCenterAPI.Dtos.ClassStudentDto", b =>
                {
                    b.Property<int>("ClassId")
                        .HasColumnType("int");

                    b.Property<string>("Grade")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IsPresent")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Teacher")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.ToTable((string)null);

                    b.ToView(null, (string)null);
                });

            modelBuilder.Entity("EducationCenterAPI.Database.Entities.Attendance", b =>
                {
                    b.HasOne("EducationCenterAPI.Database.Entities.Class", "Class")
                        .WithMany()
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EducationCenterAPI.Database.Entities.Student", "Student")
                        .WithMany()
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Class");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("EducationCenterAPI.Database.Entities.Class", b =>
                {
                    b.HasOne("EducationCenterAPI.Database.Entities.SubjectTeacher", "SubjectTeacher")
                        .WithMany()
                        .HasForeignKey("SubjectTeacherId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("SubjectTeacher");
                });

            modelBuilder.Entity("EducationCenterAPI.Database.Entities.Expense", b =>
                {
                    b.HasOne("EducationCenterAPI.Database.Entities.ExpenseType", "ExpenseType")
                        .WithMany()
                        .HasForeignKey("ExpenseTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ExpenseType");
                });

            modelBuilder.Entity("EducationCenterAPI.Database.Entities.OtherExpense", b =>
                {
                    b.HasOne("EducationCenterAPI.Database.Entities.Expense", "Expense")
                        .WithMany()
                        .HasForeignKey("ExpenseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Expense");
                });

            modelBuilder.Entity("EducationCenterAPI.Database.Entities.Student", b =>
                {
                    b.HasOne("EducationCenterAPI.Database.Entities.Grade", "Grade")
                        .WithMany("Students")
                        .HasForeignKey("GradeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Grade");
                });

            modelBuilder.Entity("EducationCenterAPI.Database.Entities.StudentFee", b =>
                {
                    b.HasOne("EducationCenterAPI.Database.Entities.Expense", "Expense")
                        .WithMany()
                        .HasForeignKey("ExpenseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EducationCenterAPI.Database.Entities.Student", "Student")
                        .WithMany()
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Expense");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("EducationCenterAPI.Database.Entities.StudentSubjectsTeachers", b =>
                {
                    b.HasOne("EducationCenterAPI.Database.Entities.Student", "Student")
                        .WithMany("StudentSubjectsTeachers")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("EducationCenterAPI.Database.Entities.SubjectTeacher", "SubjectTeacher")
                        .WithMany()
                        .HasForeignKey("SubjectTeacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Student");

                    b.Navigation("SubjectTeacher");
                });

            modelBuilder.Entity("EducationCenterAPI.Database.Entities.Subject", b =>
                {
                    b.HasOne("EducationCenterAPI.Database.Entities.Grade", "Grade")
                        .WithMany("Subjects")
                        .HasForeignKey("GradeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Grade");
                });

            modelBuilder.Entity("EducationCenterAPI.Database.Entities.SubjectTeacher", b =>
                {
                    b.HasOne("EducationCenterAPI.Database.Entities.Subject", "Subject")
                        .WithMany()
                        .HasForeignKey("SubjectsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EducationCenterAPI.Database.Entities.Teacher", "Teacher")
                        .WithMany()
                        .HasForeignKey("TeachersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subject");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("EducationCenterAPI.Database.Entities.TeacherSalary", b =>
                {
                    b.HasOne("EducationCenterAPI.Database.Entities.Expense", "Expense")
                        .WithMany()
                        .HasForeignKey("ExpenseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EducationCenterAPI.Database.Entities.Teacher", "Teacher")
                        .WithMany()
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Expense");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("EducationCenterAPI.Database.Entities.Grade", b =>
                {
                    b.Navigation("Students");

                    b.Navigation("Subjects");
                });

            modelBuilder.Entity("EducationCenterAPI.Database.Entities.Student", b =>
                {
                    b.Navigation("StudentSubjectsTeachers");
                });
#pragma warning restore 612, 618
        }
    }
}
