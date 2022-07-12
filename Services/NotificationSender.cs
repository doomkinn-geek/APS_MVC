using APS_MVC.Models;
using ASP_MVC;
using MailKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace APS_MVC.Services
{
    public class NotificationSender : INotificationSender
    {
		public SmtpConfig _smtpConfig;
		public NotificationSender(IOptions<SmtpConfig> options)
        {
			_smtpConfig = options.Value;
        }
        bool INotificationSender.SendEMail(Product product)
        {
            MimeMessage email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_smtpConfig.FromAddress));
            email.To.Add(MailboxAddress.Parse(_smtpConfig.UserName));
            email.Subject = "Уведомление о добавлении продукта";
            email.Body = new TextPart()
            {
                Text = $"Продукт {product.Id}:{product.Name} добавлен!"
            };

			SmtpClient client = new();
			try
			{
				client.Connect(_smtpConfig.Host, _smtpConfig.Port, true);
				client.Authenticate(_smtpConfig.UserName, _smtpConfig.Password);
				client.Send(email);					
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
			finally
			{
				client.Disconnect(true);
				client.Dispose();
			}
			return true;
        }
    }
}
