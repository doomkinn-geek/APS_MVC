using APS_MVC.Models;
using MailKit;
using MailKit.Net.Smtp;
using MimeKit;

namespace APS_MVC.Services
{
    public class NotificationSender : INotificationSender
    {
        bool INotificationSender.SendEMail(Product product)
        {
            MimeMessage email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("doomkinn@test.ru"));
            email.To.Add(MailboxAddress.Parse("asp2022gb@rodion-m.ru"));
            email.Subject = "Уведомление о добавлении продукта";
            email.Body = new TextPart()
            {
                Text = $"Продукт {product.Id}:{product.Name} добавлен!"
            };

			SmtpClient client = new();
			try
			{
				client.Connect("smtp.beget.com", 25, true);
				client.Authenticate("asp2022gb@rodion-m.ru", "3drtLSa1");
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
