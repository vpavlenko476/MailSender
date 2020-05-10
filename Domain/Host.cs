using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
	/// <summary>
	/// Почтовый хост
	/// </summary>
	public class Host
	{
		/// <summary>
		/// Идентификатор
		/// </summary>
		public int Id { get; set; }

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
