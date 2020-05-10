using Entities.Abstract;
using System.ComponentModel.DataAnnotations.Schema;


namespace Entities
{
	public class Recipient2MessageEntity: EntityBase
	{
		/// <summary>
		/// Получатель
		/// </summary>
		public int RecipientId { get; set; }
		[ForeignKey(nameof(RecipientId))]
		public virtual RecipientEntity Recipient { get; set; }

		/// <summary>
		/// Письмо
		/// </summary>
		public int MessageId { get; set; }
		[ForeignKey(nameof(MessageId))]
		public virtual MessageEntity Message { get; set; }
		
	}
}
