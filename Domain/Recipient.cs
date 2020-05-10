using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
	/// <summary>
	/// Получатель
	/// </summary>
	public class Recipient
	{
		public int Id { get; set; }
		/// <summary>
		/// Имя
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Адрес эл. почты
		/// </summary>
		public string Email { get; set; }

		public override string ToString()
		{
			return Email;
		}
	}
}
