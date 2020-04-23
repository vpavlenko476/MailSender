using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using MailSender.DAL.Models;

namespace MailSender.DAL.Services
{
	public class EmailSendService: IEmailSendService
	{
		///<inheritdoc cref="IEmailSendService.SendEmail(SmtpClient, Message, string, string)"/>
		public async Task SendEmail(SmtpClient client, Message message, string login, string password)
		{
			MailMessage mailMessage = new MailMessage(message.From, message.To);
			mailMessage.Subject = message.Title;
			mailMessage.Body = message.Body;
			mailMessage.IsBodyHtml = false;
			
			client.EnableSsl = true;
			client.DeliveryMethod = SmtpDeliveryMethod.Network;
			client.UseDefaultCredentials = false;
			client.Credentials = new NetworkCredential(login, password);
			client.Send(mailMessage);
		}
	}
}
