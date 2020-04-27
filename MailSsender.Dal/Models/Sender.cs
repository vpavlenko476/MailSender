using MailSender.DAL.Models.Base;
using System;

namespace MailSender.DAL.Models
{
	/// <summary>
	/// Отправитель письма
	/// </summary>
	public class Sender: EntityBase
	{
		/// <summary>
		/// Адрес эл. почты
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// Зашифрованный пароль
		/// </summary>
		public string Password { get; set; }
	}
}
