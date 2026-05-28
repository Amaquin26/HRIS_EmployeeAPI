using HRIS_Employee.API.External.Graph;
using HRIS_Employee.API.Middlewares;
using HRIS_Employee.API.Repositories;
using HRIS_Employee.API.Repositories.Base;
using HRIS_Employee.API.Repositories.Employee;
using HRIS_Employee.API.Repositories.Schedule;
using HRIS_Employee.API.Services;
using HRIS_Employee.API.Services.Schedule;
using HRIS_Employee.API.Services.UnitOfWork;
using HRIS_Employee.Infrastructure.Persistence.DBContext;
using HRIS_Employee.Infrastructure.Persistence.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<EmployeeDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

builder.Services.AddCors(opt =>
{
    opt.AddPolicy(name: "CorsPolicy", builder =>
    {
        builder.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddMicrosoftIdentityWebApi(options =>
{
    options.TokenValidationParameters.NameClaimType = "name";
},
options =>
{
    builder.Configuration.Bind("AzureAd", options);
});

builder.Services.AddAuthorization();

builder.Services.Configure<AzureAdSettings>(builder.Configuration.GetSection("AzureAd"));
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IEmployeeStatusRepository, EmployeeStatusRepository>();
builder.Services.AddScoped<IEmployeeStatusService, EmployeeStatusService>();
builder.Services.AddScoped<IAuthUserDetailsService, AuthUserDetailsService>();
builder.Services.AddScoped<IScheduleRepository, ScheduleRepository>();
builder.Services.AddScoped<IScheduleService, ScheduleService>();
builder.Services.AddScoped<IGraphService, GraphService>();
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("CorsPolicy");

app.UseGlobalExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
