using MailSender.DAL.Models;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MailSender.DAL.Services
{
	/// <summary>
	/// Функционал рассылки писем
	/// </summary>
	public interface IEmailSendService
	{
		/// <summary>
		/// Отправить письмо сейчас
		/// </summary>
		/// <param name="client">Smtp клиент</param>
		/// <param name="message">Объект письма</param>		
		Task SendEmailNow(string smtpServer, IEnumerable<MailMessage> mailMessages);

		/// <summary>
		/// Отправить письмо запланированно
		/// </summary>
		/// <param name="client">Smtp клиент</param>
		/// <param name="message">Объект письма</param>		
		void SendEmailScheduler(string smtpServer, IEnumerable<MailMessage> mailMessages, DateTime date, DateTime time);
	}
}
