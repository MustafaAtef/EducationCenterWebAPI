using EducationCenterAPI.Database;
using EducationCenterAPI.Exceptions;
using EducationCenterAPI.Options;
using EducationCenterAPI.Repositories;
using EducationCenterAPI.RepositoryContracts;
using EducationCenterAPI.ServiceContracts;
using EducationCenterAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("jwt"));

builder.Services.AddTransient<IGradesService, GradesService>();
builder.Services.AddTransient<ISubjectsService, SubjectsService>();
builder.Services.AddTransient<IJwtService, JwtService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<ITeachersService, TeachersService>();
builder.Services.AddTransient<IStudentsService, StudentsService>();
builder.Services.AddTransient<IClassesService, ClassesService>();
builder.Services.AddTransient<IAttendanceService, AttendanceService>();
builder.Services.AddTransient<IExpenseService, ExpenseService>();
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var jwtOptions = builder.Configuration.GetSection("jwt").Get<JwtOptions>();
builder.Services.AddAuthentication().AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    if (jwtOptions is null) throw new Exception();
    // save the authentication token to authintication properties so it can be accessed from httpContext object
    options.SaveToken = true;
    options.MapInboundClaims = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidIssuer = jwtOptions.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtOptions.Audience,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey)),
        ValidateLifetime = true,
        // The default value of ClockSkew is 5 minutes. That means if you haven't set it, your token will be still valid for up to 5 minutes. If you want to expire your token on the exact time; you'd need to set ClockSkew to zero
        ClockSkew = TimeSpan.Zero,
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseGlobalErrorHandling();

app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

app.Run();
