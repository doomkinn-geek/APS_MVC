using APS_MVC.Models;
using ASP_MVC.Configuration;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using Serilog;

namespace APS_MVC.Services
{
    public class NotificationSender : INotificationSender
    {
		public SmtpConfig _smtpConfig;
		public SmtpSecurityConfig _smtpSecurityConfig;
		public NotificationSender(IOptions<SmtpConfig> options, IOptions<SmtpSecurityConfig> smtpSecurityConfig)
        {
            _smtpConfig = options.Value;
            _smtpSecurityConfig = smtpSecurityConfig.Value;
        }
        bool INotificationSender.SendEMail(Product product)
        {
			Log.Information("Отправляем уведомление о добавлении продукта на e-mail");
            MimeMessage email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_smtpConfig.FromAddress));
            email.To.Add(MailboxAddress.Parse(_smtpSecurityConfig.UserName));
            email.Subject = "Уведомление о добавлении продукта";
            email.Body = new TextPart()
            {
                Text = $"Продукт {product.Id}:{product.Name} добавлен!"
            };

			SmtpClient client = new();
			try
			{
				client.Connect(_smtpConfig.Host, _smtpConfig.Port, true);
				client.Authenticate(_smtpSecurityConfig.UserName, _smtpSecurityConfig.Password);
				client.Send(email);					
			}
			catch (Exception ex)
			{
				Log.Error(ex.Message);				
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
