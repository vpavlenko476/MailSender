using MailSender.DAL.Models.Base;
using System;
using System.Collections.Generic;

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

		/// <summary>
		/// Сообщение
		/// </summary>
		public virtual ICollection<Message> Messages { get; set; }

		public override string ToString()
		{
			return Email;
		}
	}
}
