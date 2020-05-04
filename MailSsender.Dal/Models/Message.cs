
using MailSender.DAL.Models.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MailSender.DAL.Models
{	
	public class Message: EntityBase
	{
		/// <summary>
		/// Отправитель
		/// </summary>
		public int SenderId { get; set; }
		[ForeignKey(nameof(SenderId))]
		public virtual Sender Sender { get; set; }
		
		/// <summary>
		/// Получатели
		/// </summary>
		public ICollection<Recipient> Recipients { get; set; }
		public Message()
		{
			Recipients = new List<Recipient>();
		}

		/// <summary>
		/// Заголовок
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// Сообщение
		/// </summary>
		public string Body { get; set; }

		/// <summary>
		/// Время отправки
		/// </summary>
		[Timestamp]
		public byte[] TimeStamp { get; set; }
	}
}
