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
using MailSender.DAL.Exceptions;

namespace MailSender.DAL.Services
{
	public class EmailSendService : IEmailSendService
	{
		///<inheritdoc cref="IEmailSendService.SendEmail(SmtpClient, Message)"/>
		public async Task SendEmailNow(string smtpServer, IEnumerable<MailMessage> mailMessages)
		{
			try
			{
				DataValidate(smtpServer, mailMessages);
			}
			catch (EmailSendServiceException ex)
			{
				throw ex;
			}
			var dbRecipient = new BaseRepo<Recipient>();
			var dbHost = new BaseRepo<Host>();
			var dbMessage = new BaseRepo<Message>();
			var dbSender = new BaseRepo<Sender>();
			var dbRecipientsMessage = new BaseRepo<RecipientMessage>();
			var sender = dbSender.GetAll().Where(x => x.Email == mailMessages.FirstOrDefault().From.Address).FirstOrDefault();
			var allRecipients = dbRecipient.GetAll();
			var sendRecipients = new List<Recipient>();

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

				client.Credentials = new NetworkCredential(sender.Email, PasswordCoding.Decrypt(sender.Password));

				foreach (var mail in mailMessages)
				{
					var recipient = allRecipients.Where(x => x.Email == mail.To.FirstOrDefault().Address).FirstOrDefault();
					sendRecipients.Add(recipient);
					await client.SendMailAsync(mail);
				}
			}

			dbMessage.Add(new Message()
			{
				SenderId = sender.Id,
				Body = mailMessages.FirstOrDefault().Body,
				Title = mailMessages.FirstOrDefault().Subject
			});

			foreach (var rec in sendRecipients)
			{
				dbRecipientsMessage.Add(new RecipientMessage()
				{
					MessageId = dbMessage.GetAll().Select(x => x.Id).LastOrDefault(),
					RecipientId = rec.Id
				});
			}
		}

		///<inheritdoc cref="IEmailSendService.SendEmailScheduler(string, MailMessage, DateTime, TimeSpan)"/>
		public void SendEmailScheduler(string smtpServer, IEnumerable<MailMessage> mailMessages, DateTime date, DateTime time)
		{
			try
			{
				DataValidate(smtpServer, mailMessages, date);
			}
			catch (EmailSendServiceException ex)
			{
				throw ex;
			}
			var timer = new System.Timers.Timer(60000);
			var sendDateTime = date.Add(TimeSpan.Parse(time.ToString("HH:mm")));
			timer.Elapsed += async (sender, args) => await OnTimeoutAsync(sender, smtpServer, mailMessages, sendDateTime);
			timer.Enabled = true;
		}

		private async Task OnTimeoutAsync(Object source, string smtpServer, IEnumerable<MailMessage> mailMessages, DateTime sendDateTime)
		{
			if (sendDateTime.ToShortTimeString() == DateTime.Now.ToShortTimeString())
			{
				await SendEmailNow(smtpServer, mailMessages);
			}
		}

		private void DataValidate(string smtpServer, IEnumerable<MailMessage> mailMessages)
		{
			if (smtpServer == null) throw new EmailSendServiceException("Укажите smtp-сервер");
			if (mailMessages.Count() == 0) throw new EmailSendServiceException("Укажите отправителя/получателя");
			var message = mailMessages.First();
			if (message.From == null) throw new EmailSendServiceException("Укажите отправителя");
			else if (message.To == null) throw new EmailSendServiceException("Укажите Получателя");
		}

		private void DataValidate(string smtpServer, IEnumerable<MailMessage> mailMessages, DateTime date)
		{			
			if (smtpServer == null) throw new EmailSendServiceException("Укажите smtp-сервер");
			if (mailMessages.Count() == 0) throw new EmailSendServiceException("Укажите отправителя/получателя");
			var message = mailMessages.First();
			if (message.From == null) throw new EmailSendServiceException("Укажите отправителя");
			if (message.To == null) throw new EmailSendServiceException("Укажите Получателя");
			if (date < DateTime.Now) throw new EmailSendServiceException("Вы указали дату в прошлом");
		}
	}
}
