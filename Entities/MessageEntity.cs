using Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{	
	public class MessageEntity: EntityBase
	{
		/// <summary>
		/// Отправитель
		/// </summary>
		public int SenderId { get; set; }
		[ForeignKey(nameof(SenderId))]
		public virtual SenderEntity Sender { get; set; }
		
		/// <summary>
		/// Получатели
		/// </summary>
		public ICollection<RecipientEntity> Recipients { get; set; }
		public MessageEntity()
		{
			Recipients = new List<RecipientEntity>();
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
		/// Запланированное время отправления
		/// </summary>
		public DateTime? ScheduledSendDateTime { get; set; }

		/// <summary>
		/// Фактическое время отправления
		/// </summary>
		public DateTime? SendDateTime { get; set; }
	}
}
