using MailSender.DAL.Models;
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
		/// Отправить письмо
		/// </summary>
		/// <param name="client">Smtp клиент</param>
		/// <param name="message">Объект письма</param>
		/// <param name="login">Логин отправителя</param>
		/// <param name="password">Пароль отправителя</param>
		/// <returns></returns>
		Task SendEmail(SmtpClient client, Message message, string login, string password);
	}
}
