namespace HMS_Final.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email,string subject,string message);
    }
}
