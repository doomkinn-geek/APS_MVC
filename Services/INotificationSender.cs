using APS_MVC.Models;

namespace APS_MVC.Services
{
    public interface INotificationSender
    {
        public bool SendEMail(Product product);
        public Task SendEmailAsync(Product product, CancellationToken cancellationToken);
    }
}
