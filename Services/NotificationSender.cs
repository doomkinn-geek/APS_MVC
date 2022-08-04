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
            var emailMessage = MakeMessage(product);
            Log.Information("Отправляем уведомление о добавлении продукта на e-mail");
			SmtpClient client = new();
			try
			{
				client.Connect(_smtpConfig.Host, _smtpConfig.Port, true);
				client.Authenticate(_smtpSecurityConfig.UserName, _smtpSecurityConfig.Password);
				client.Send(emailMessage);					
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

		async Task INotificationSender.SendEmailAsync(Product product)
        {
            var emailMessage = MakeMessage(product);
            Log.Information("Асинхронно отправляем уведомление о добавлении продукта на e-mail");
            try
            {
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_smtpConfig.Host, _smtpConfig.Port, false);
                    await client.AuthenticateAsync(_smtpSecurityConfig.UserName, _smtpSecurityConfig.Password);
                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);                
            }
        }

        private MimeMessage MakeMessage(Product product)
        {
            MimeMessage email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_smtpConfig.FromAddress));
            email.To.Add(MailboxAddress.Parse(_smtpSecurityConfig.UserName));
            email.Subject = "Уведомление о добавлении продукта";
            email.Body = new TextPart()
            {
                Text = $"Продукт {product.Id}:{product.Name} добавлен!"
            };
            return email;
        }
    }
}
