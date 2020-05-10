using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
	public class Sender
	{
		/// <summary>
		/// Идентификатор
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Адрес эл. почты
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// Зашифрованный пароль
		/// </summary>
		public string Password { get; set; }

		public override string ToString()
		{
			return Email;
		}
	}
}
