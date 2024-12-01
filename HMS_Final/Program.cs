using AppointmentManagementSystem.Data;
using HMS_Final.Manager;
using HMS_Final.Repository;
using HMS_Final.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using HMS_Final.Manager.Hospitals;
using HMS_Final.Controllers.Hospitals;
using HMS_Final.Manager.Countries;
using HMS_Final.Controllers.Countries;
using HMS_Final.Manager.Cities;
using HMS_Final.Manager.Departments;
using Microsoft.Extensions.DependencyInjection;
using HMS_Final.Manager.Doctors;
using HMS_Final.Manager.Schedules;
using HMS_Final.Services;
using HMS_Final.Manager.Users;
using HMS_Final.Manager.Insurances;
using HMS_Final.Manager.Feedbacks;
using HMS_Final.Manager.Appointments;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add Controllers

// Add Controllers
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore); // Prevent reference loop issues
//Add Email Service
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddSingleton<IExpiredOTPService, ExpiredOTPService>();
builder.Services.AddHostedService<ExpiredOTPService>();

// Configure DbContext with SQL Server
builder.Services.AddDbContext<AMSDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Repository and Generic Repository
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>)); // Generic repository for any model

// Register Manager and Manager Interfaces

builder.Services.AddScoped<ICountryManager, CountryManager>(); // For country-specific manager
builder.Services.AddScoped<ICityManager, CityManager>(); // For city-specific manager
builder.Services.AddScoped<IHospitalManager, HospitalManager>(); // For Hospital-specific manager
builder.Services.AddScoped<IDepartmentManager,DepartmentManager>(); // For department-specific manager
builder.Services.AddScoped<IDoctorManager,DoctorManager>(); // For doctor-specific manager
builder.Services.AddScoped<IScheduleManager,ScheduleManager>(); // For Schedule-specific manager
builder.Services.AddScoped<IUserManager, UserManager>(); //For user-specific manager
builder.Services.AddScoped<IInsuranceManager,InsuranceManager>();
builder.Services.AddScoped<IFeedbackManager, FeedbackManager>();
builder.Services.AddScoped<IAppointmentManager,AppointmentManager>();

// Register AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); // Automatically scans for mapping profiles
//builder.Services.AddAutoMapper(typeof(CountryMappingProfile));
//builder.Services.AddAutoMapper(typeof(CountryMappingProfile));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
