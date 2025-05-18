using EducationCenter.Application.ServiceContracts;
using EducationCenter.Application.Services;
using EducationCenter.Core.RepositoryContracts;
using EducationCenter.Infrastructure.Database;
using EducationCenter.WebApi.Middlewares;
using EducationCenter.Infrastructure;
using EducationCenterAPI.Repositories;
using EducationCenterAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddMvcOptions(options =>
{
    options.Filters.Add(new ProducesAttribute("application/json"));
    options.Filters.Add(new ConsumesAttribute("application/json"));
});
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
    // save the authentication token to authentication properties so it can be accessed from httpContext object
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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    c.SwaggerDoc("v1", new() { Title = "EducationCenterAPI", Version = "v1" });
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(dpb =>
    {
        dpb.WithOrigins("http://localhost:5000")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseGlobalErrorHandling();

app.UseHttpsRedirection();

app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EducationCenterAPI v1"));
}

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
