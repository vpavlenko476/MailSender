using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using MailSender.DAL.Models;
using MailSender.DAL.Repos;
using System.Linq;
using System.Threading;
using System.Timers;

namespace MailSender.DAL.Services
{
	public class EmailSendService : IEmailSendService
	{
		///<inheritdoc cref="IEmailSendService.SendEmail(SmtpClient, Message)"/>
		public async Task SendEmailNow(string smtpServer, MailMessage message)
		{			
			var dbRecipient = new BaseRepo<Recipient>();
			var dbHost = new BaseRepo<Host>();
			var recipient = dbRecipient.GetAll()
				.Where(x => x.Email == message.To.FirstOrDefault().Address)
				.FirstOrDefault();			

			using (var client = new SmtpClient())
			{
				client.Host = smtpServer;
				client.Port = dbHost.GetAll()
					.Where(x => x.Server == smtpServer)
					.Select(x => x.Port)
					.FirstOrDefault();
				client.EnableSsl = true;
				client.DeliveryMethod = SmtpDeliveryMethod.Network;
				client.UseDefaultCredentials = false;

				var dbSender = new BaseRepo<Sender>();
				var sender = dbSender.GetAll()
					.Where(x => x.Email == message.From.Address)
					.FirstOrDefault();				
				client.Credentials = new NetworkCredential(sender.Email, PasswordCoding.Decrypt(sender.Password));

				await client.SendMailAsync(message);				
			}				
		}

		///<inheritdoc cref="IEmailSendService.SendEmailScheduler(string, MailMessage, DateTime, TimeSpan)"/>
		public void SendEmailScheduler(string smtpServer, MailMessage message, DateTime date, DateTime time)
		{
			var timer = new System.Timers.Timer(1000);
			var sendDateTime = date.Add(TimeSpan.Parse(time.ToString("HH:mm")));
			timer.Elapsed += async (sender, args) => await OnTimeoutAsync(sender, smtpServer, message, sendDateTime);
		}

		private async Task OnTimeoutAsync(Object source, string smtpServer, MailMessage message, DateTime sendDateTime)
		{
			if(sendDateTime.ToShortTimeString()== DateTime.Now.ToShortTimeString())
			{
				await SendEmailNow(smtpServer, message);
			}
		}
	}
}
