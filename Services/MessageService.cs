using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using System.Linq;
using Data.Repositories.Abstract;
using Entities;
using Services.Abstract;
using Services.Exceptions;
using Domain;
using Mapper;

namespace Services
{
	public class MessageService : IMessageSendService
	{
		IBaseRepo<MessageEntity> _messageRepository;
		IBaseRepo<SenderEntity> _senderRepository;
		IBaseRepo<RecipientEntity> _recipientRepository;
		IBaseRepo<HostEntity> _hostRepository;
		IBaseRepo<Recipient2MessageEntity> _recipient2MessageRepository;
		public Action messageScheduled;

		public MessageService
			(IBaseRepo<MessageEntity> messageRepository, 
			IBaseRepo<SenderEntity> senderRepository, 
			IBaseRepo<RecipientEntity> recRepository,
			IBaseRepo<HostEntity> hostRepository,
			IBaseRepo<Recipient2MessageEntity> recipient2MessageRepository)
		{
			_messageRepository = messageRepository;
			_senderRepository = senderRepository;
			_recipientRepository = recRepository;
			_hostRepository = hostRepository;
			_recipient2MessageRepository = recipient2MessageRepository;
		}
		///<inheritdoc cref="IMessageSendService.SendEmail(SmtpClient, Message)"/>
		public async Task SendEmailNow(string smtpServer, IEnumerable<MailMessage> mailMessages, bool isSheduled = false)
		{
			if(isSheduled==false)
			{
				try
				{
					DataValidate(smtpServer, mailMessages);
				}
				catch (EmailSendServiceException ex)
				{
					throw ex;
				}
			}			
			
			var sender = _senderRepository.GetAll().Where(x => x.Email == mailMessages.FirstOrDefault().From.Address).FirstOrDefault().ToDomain();
			var allRecipients = _recipientRepository.GetAll();
			var messageRecipients = new List<Recipient>();

			using (var client = new SmtpClient())
			{
				client.Host = smtpServer;
				client.Port = _hostRepository.GetAll()
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
					messageRecipients.Add(recipient.ToDomain());
					await client.SendMailAsync(mail);
				}
			}

			if(isSheduled==false)
			{
				var sendedMessage = new Message()
				{
					SenderId = sender.Id,
					Body = mailMessages.FirstOrDefault().Body,
					Title = mailMessages.FirstOrDefault().Subject,
					SendDateTime = DateTime.Now
				};
				_messageRepository.Add(sendedMessage.ToEntity());
			};

			foreach (var rec in messageRecipients)
			{
				_recipient2MessageRepository.Add(new Recipient2MessageEntity()
				{
					MessageId = _messageRepository.GetAll().Select(x => x.Id).LastOrDefault(),
					RecipientId = rec.Id
				});
			}
		}

		///<inheritdoc cref="IMessageSendService.SendEmailScheduler(string, MailMessage, DateTime, TimeSpan)"/>
		public void SendEmailScheduler(string smtpServer, IEnumerable<MailMessage> mailMessages, DateTime date, DateTime time)
		{
			var scheduledSendDateTime = date.Add(TimeSpan.Parse(time.ToString("HH:mm")));
			try
			{
				DataValidate(smtpServer, mailMessages, scheduledSendDateTime);
			}
			catch (EmailSendServiceException ex)
			{
				throw ex;
			}						
			
			var sheduledMessage = new Message()
			{
				SenderId = _senderRepository.GetAll().Where(x => x.Email == mailMessages.FirstOrDefault().From.Address).FirstOrDefault().Id,
				Body = mailMessages.FirstOrDefault().Body,
				Title = mailMessages.FirstOrDefault().Subject,
				ScheduledSendDateTime = scheduledSendDateTime,
				SendDateTime = null
			};
			_messageRepository.Add(sheduledMessage.ToEntity());
			//messageScheduled.Invoke();

			var timer = new System.Timers.Timer(1000)
			{
				Enabled = true
			};
			timer.Elapsed += (sender, args) => OnTimeoutAsync2(sender, smtpServer, mailMessages, scheduledSendDateTime);
			timer.Start();

		}

		private void OnTimeoutAsync2(Object source, string smtpServer, IEnumerable<MailMessage> mailMessages, DateTime scheduledSendDateTime)
		{
			if (scheduledSendDateTime.ToShortTimeString() == DateTime.Now.ToShortTimeString())
			{				
				var sendedEmail = _messageRepository.GetAll().Where(x => x.ScheduledSendDateTime == scheduledSendDateTime).FirstOrDefault().ToDomain();
				sendedEmail.SendDateTime = DateTime.Now;
				sendedEmail.ScheduledSendDateTime = null;
				_messageRepository.Save(sendedEmail.ToEntity());
			}
		}
		private async void OnTimeoutAsync(Object source, string smtpServer, IEnumerable<MailMessage> mailMessages, DateTime scheduledSendDateTime)
		{
			if (scheduledSendDateTime.ToShortTimeString() == DateTime.Now.ToShortTimeString())
			{
				await SendEmailNow(smtpServer, mailMessages, true);
				var sendedEmail = _messageRepository.GetAll().Where(x => x.ScheduledSendDateTime == scheduledSendDateTime).FirstOrDefault().ToDomain();
				sendedEmail.SendDateTime = DateTime.Now;
				sendedEmail.ScheduledSendDateTime = null;
				_messageRepository.Save(sendedEmail.ToEntity());
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
			if (date.Date < DateTime.Now.Date) throw new EmailSendServiceException("Вы указали дату в прошлом");
		}
	}
}
