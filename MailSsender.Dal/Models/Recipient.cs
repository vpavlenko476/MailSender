using MailSender.DAL.Models.Base;
using System.Collections.Generic;

namespace MailSender.DAL.Models
{
	/// <summary>
	/// Получатель письма
	/// </summary>
	public partial class Recipient: EntityBase
	{
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
		public ICollection<Message> Messages { get; set; }
		public Recipient()
		{
			Messages = new List<Message>();
		}
		public override string ToString()
		{
			return Email;
		}
	}
}
