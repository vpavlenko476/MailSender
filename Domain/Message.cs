using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
	public class Message
	{
		/// <summary>
		/// Идентификатор
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Идентификатор отправителя
		/// </summary>
		public int SenderId { get; set; }

		/// <summary>
		/// Заголовок
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// Сообщение
		/// </summary>
		public string Body { get; set; }

		/// <summary>
		/// Запланированное время отправления
		/// </summary>
		public DateTime? ScheduledSendDateTime { get; set; }

		/// <summary>
		/// Фактическое время отправления
		/// </summary>
		public DateTime? SendDateTime { get; set; }
	}
}
