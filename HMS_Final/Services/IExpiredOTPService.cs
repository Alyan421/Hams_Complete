using System.Threading;
using System.Threading.Tasks;

namespace HMS_Final.Services
{
    public interface IExpiredOTPService
    {
        Task StartAsync(CancellationToken cancellationToken);
        Task StopAsync(CancellationToken cancellationToken);
    }
} 