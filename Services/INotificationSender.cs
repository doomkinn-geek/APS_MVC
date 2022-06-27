using APS_MVC.Models;

namespace APS_MVC.Services
{
    public interface INotificationSender
    {
        public bool SendEMail(Product product);        
    }
}
