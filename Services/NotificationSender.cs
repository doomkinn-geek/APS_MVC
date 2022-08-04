using APS_MVC.Models;
using ASP_MVC.Configuration;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using Serilog;

namespace APS_MVC.Services
{
    public class NotificationSender : INotificationSender, IAsyncDisposable
    {
		public SmtpConfig _smtpConfig;
		public SmtpSecurityConfig _smtpSecurityConfig;
        public SmtpClient _smtpClient;
        public NotificationSender(IOptions<SmtpConfig> options, IOptions<SmtpSecurityConfig> smtpSecurityConfig)
        {
            _smtpConfig = options.Value;
            _smtpSecurityConfig = smtpSecurityConfig.Value;
            _smtpClient = new();
        }
        bool INotificationSender.SendEMail(Product product)
        {
            var emailMessage = MakeMessage(product);
            Log.Information("Отправляем уведомление о добавлении продукта на e-mail");			
			try
			{
                _smtpClient.Connect(_smtpConfig.Host, _smtpConfig.Port, true);
                _smtpClient.Authenticate(_smtpSecurityConfig.UserName, _smtpSecurityConfig.Password);
                _smtpClient.Send(emailMessage);					
			}
			catch (Exception ex)
			{
				Log.Error(ex.Message);				
				return false;
			}
			finally
			{
                _smtpClient.Disconnect(true);
                _smtpClient.Dispose();
			}
			return true;
        }

		async Task INotificationSender.SendEmailAsync(Product product, CancellationToken cancellationToken)
        {
            var emailMessage = MakeMessage(product);
            Log.Information("Асинхронно отправляем уведомление о добавлении продукта на e-mail");
            try
            {
                await _smtpClient.ConnectAsync(_smtpConfig.Host, _smtpConfig.Port, false, cancellationToken);
                await _smtpClient.AuthenticateAsync(_smtpSecurityConfig.UserName, _smtpSecurityConfig.Password, cancellationToken);
                await _smtpClient.SendAsync(emailMessage, cancellationToken);
                //await client.DisconnectAsync(true);                
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

        public async ValueTask DisposeAsync()
        {
            if (_smtpClient.IsConnected)
            {
                await _smtpClient.DisconnectAsync(true);
            }
            _smtpClient.Dispose();
        }
    }
}
