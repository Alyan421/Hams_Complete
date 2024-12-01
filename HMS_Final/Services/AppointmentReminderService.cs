using HMS_Final.Models;
using HMS_Final.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HMS_Final.Services
{
    public class AppointmentReminderService : BackgroundService, IAppointmentReminderService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<AppointmentReminderService> _logger;

        public AppointmentReminderService(IServiceProvider serviceProvider, ILogger<AppointmentReminderService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await SendRemindersAsync();
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken); // Check every hour
            }
        }

        public async Task SendRemindersAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var appointmentRepository = scope.ServiceProvider.GetRequiredService<IGenericRepository<Appointment>>();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                var upcomingAppointments = await appointmentRepository.GetDbSet()
                    .Where(a => a.AppointmentDateTime > DateTime.Now && a.AppointmentDateTime <= DateTime.Now.AddHours(24))
                    .Include(a => a.User) // Ensure User is included
                    .ToListAsync();

                foreach (var appointment in upcomingAppointments)
                {
                    var user = appointment.User;
                    if (user != null)
                    {
                        string subject = "Appointment Reminder";
                        string message = $"Dear {user.UserName},\n\nThis is a reminder for your appointment scheduled on {appointment.AppointmentDateTime}.\n\nThank you.";
                        try
                        {
                            await emailService.SendEmailAsync(user.Email, subject, message);
                            _logger.LogInformation($"Reminder sent to {user.Email} for appointment on {appointment.AppointmentDateTime}");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"Failed to send reminder to {user.Email}");
                        }
                    }
                }
            }
        }
    }
} 