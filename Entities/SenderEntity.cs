using Entities.Abstract;
using System.Collections.Generic;

namespace Entities
{
	/// <summary>
	/// Отправитель письма
	/// </summary>
	public class SenderEntity: EntityBase
	{
		/// <summary>
		/// Адрес эл. почты
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// Зашифрованный пароль
		/// </summary>
		public string Password { get; set; }

		/// <summary>
		/// Сообщение
		/// </summary>
		public virtual ICollection<MessageEntity> Messages { get; set; }
	}
}
