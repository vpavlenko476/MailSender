using MailSender.DAL.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MailSender.DAL.Models
{
	public class RecipientMessage: EntityBase
	{
		/// <summary>
		/// Получатель
		/// </summary>
		public int RecipientId { get; set; }
		[ForeignKey(nameof(RecipientId))]
		public virtual Recipient Recipient { get; set; }

		/// <summary>
		/// Письмо
		/// </summary>
		public int MessageId { get; set; }
		[ForeignKey(nameof(MessageId))]
		public virtual Message Message { get; set; }
		
	}
}
