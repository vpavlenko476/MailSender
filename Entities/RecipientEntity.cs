using Entities.Abstract;
using System.Collections.Generic;

namespace Entities
{
	/// <summary>
	/// Получатель письма
	/// </summary>
	public partial class RecipientEntity: EntityBase
	{
		public RecipientEntity()
		{
			Messages = new List<MessageEntity>();
		}
		/// <summary>
		/// Имя
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Адрес эл. почты
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// Полученные письма
		/// </summary>		
		public ICollection<MessageEntity> Messages { get; set; }		
	}
}
