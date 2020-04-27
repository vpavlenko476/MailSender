using MailSender.DAL.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MailSender.DAL.Models
{
	/// <summary>
	/// Подключение к smtp-серверу
	/// </summary>
	public class Host: EntityBase
	{
		/// <summary>
		/// Сервер
		/// </summary>
		public string Server { get; set; }

		/// <summary>
		/// Порт
		/// </summary>
		public int Port { get; set; }
	}
}
